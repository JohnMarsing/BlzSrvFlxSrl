using BlazorDateRangePicker;
using Microsoft.AspNetCore.Components;
using LoginLink = BlzSrvFlxSrl.Links.Account;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Index
{
	[Inject] NavigationManager? NavigationManager { get; set; }
	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }
	[Inject] public ILogger<Index>? Logger { get; set; }

	private DateTimeOffset? DateBegin { get; set; }
	private DateTimeOffset? DateEnd { get; set; }
	private bool IsTableVisible => SpecialEventsState!.Value.IsTableVisible;
	private bool IsFormVisible => SpecialEventsState!.Value.IsFormVisible;
	private bool IsDisplayVisible => SpecialEventsState!.Value.IsDisplayVisible;
	//protected bool TurnSpinnerOff = false;  // try {... call db } catch [... deal with errors]	finally { TurnSpinnerOff = true; }

	protected override void OnInitialized()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Index) + "!" + nameof(OnInitializedAsync)));
		DateBegin = SpecialEventsState!.Value.DateBegin;
		DateEnd = SpecialEventsState!.Value.DateEnd;
		Dispatcher?.Dispatch(new SpecialEvents_GetListWithDates_Action(DateBegin, DateEnd));
		base.OnInitialized();
	}

	public void OnRangeSelect(DateRange range)
	{
		Dispatcher!.Dispatch(new SpecialEvents_SetDateRange_Action(range.Start, range.End));
		Dispatcher?.Dispatch(new SpecialEvents_GetListWithDates_Action(
			SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
	}

	void RedirectToLoginClick(string returnUrl)
	{
		NavigationManager!.NavigateTo($"{LoginLink.Login}?returnUrl={returnUrl}", true);
	}
}
