using BlazorDateRangePicker;
using Microsoft.AspNetCore.Components;
using LoginLink = BlzSrvFlxSrl.Links.Account;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Index
{
	[Inject] NavigationManager? NavigationManager { get; set; }
	[Inject] private IState<State>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }
	[Inject] public ILogger<Index>? Logger { get; set; }

	private DateTimeOffset? DateBegin { get; set; }
	private DateTimeOffset? DateEnd { get; set; }

	protected override void OnInitialized()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Index) + "!" + nameof(OnInitializedAsync)));
		DateBegin = SpecialEventsState!.Value.DateBegin;
		DateEnd = SpecialEventsState!.Value.DateEnd;
		Dispatcher?.Dispatch(new GetListWithDates_Action(DateBegin, DateEnd));
		base.OnInitialized();
	}

	public void OnRangeSelect(DateRange range)
	{
		Dispatcher!.Dispatch(new SetDateRange_Action(range.Start, range.End));
		Dispatcher?.Dispatch(new GetListWithDates_Action(
			SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
	}

	void RedirectToLoginClick(string returnUrl)
	{
		NavigationManager!.NavigateTo($"{LoginLink.Login}?returnUrl={returnUrl}", true);
	}
}
