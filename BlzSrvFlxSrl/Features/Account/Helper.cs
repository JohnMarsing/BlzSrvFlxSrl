using System.Security.Claims;
using static BlzSrvFlxSrl.Services.Auth0;

namespace BlzSrvFlxSrl.Features.Account;

public static class Helper
{
	public static string? GetUserNameSoapVersion(this ClaimsPrincipal user)
	{
		return user.Claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
	}
}