using Microsoft.AspNetCore.Components;
using LoginLink = BlzSrvFlxSrl.Links.Account;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Index
{
	[Inject] private IState<State>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }
	[Inject] public ILogger<Index>? Logger { get; set; }
	[Inject] NavigationManager? NavigationManager { get; set; }

	void RedirectToLoginClick(string returnUrl)
	{
		NavigationManager!.NavigateTo($"{LoginLink.Login}?returnUrl={returnUrl}", true);
	}
}
