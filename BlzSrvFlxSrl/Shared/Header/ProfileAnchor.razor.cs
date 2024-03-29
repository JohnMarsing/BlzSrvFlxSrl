﻿using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

using BlzSrvFlxSrl.Helpers;
using BlzSrvFlxSrl.Features.Account;

namespace BlzSrvFlxSrl.Shared.Header;

public partial class ProfileAnchor
{
	[Parameter, EditorRequired] public bool IsXsOrSm { get; set; }
	protected string CssClass => IsXsOrSm ? "mt-1" : "";

	[Inject] public AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

	public string? Name { get; set; }
	public string? EmailAddress { get; set; }
	public bool Verified { get; set; }
	public string? Role { get; set; }
	private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

	protected override async Task OnInitializedAsync()
	{
		base.OnInitialized();
		var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
		var user = authState.User;

		if (user.Identity!.IsAuthenticated)
		{
			Verified = true;
			_claims = user.Claims;
		}
		else
		{
			Verified = false;
		}

		Name = user.GetUserNameSoapVersion();
		EmailAddress = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
		Role = user.Claims.FirstOrDefault(c => c.Type == "https://schemas.livingmessiah.com/roles")?.Value;

	}

	public bool IsAdmin
	{
		get
		{
			if (Verified && Role!.Contains("admin", System.StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public string NameTiny
	{
		get
		{
			return !String.IsNullOrEmpty(Name) ? $"<span class='text-success'><b>{Name.Truncate(25)}</b></span>" : "";
		}
	}

	public string BlueCheck
	{
		get
		{
			if (Verified)
			{
				return "<span class='text-primary'><i class='fas fa-check'></i></span>";
			}
			else
			{
				return "<span class='text-danger'>Unverified<i class='fas fa-question'></i></span>";
			}
		}
	}

}

