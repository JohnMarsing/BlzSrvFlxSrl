using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Index
{
	[Inject] private IState<State>? State { get; set; }
}


/*
ToDo: add the Login logic

using LoginLink = BlzSrvFlxSrl.Links.Account;

	[Inject] public ILogger<Index>? Logger { get; set; }
	[Inject] NavigationManager? NavigationManager { get; set; }

	void RedirectToLoginClick(string returnUrl)
	{
		NavigationManager!.NavigateTo($"{LoginLink.Login}?returnUrl={returnUrl}", true);
	}

*/