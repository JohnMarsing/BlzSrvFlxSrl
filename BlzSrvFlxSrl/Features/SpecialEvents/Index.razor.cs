﻿using BlazorDateRangePicker;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using LoginLink = BlzSrvFlxSrl.Links.Account;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Index
{
	[Inject] NavigationManager? NavigationManager { get; set; }
	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	private DateTimeOffset? DateBegin { get; set; }
	private DateTimeOffset? DateEnd { get; set; }


	protected override void OnInitialized()
	{
		DateBegin = SpecialEventsState!.Value.DateBegin;
		DateEnd = SpecialEventsState!.Value.DateEnd;
		base.OnInitialized();
	}

	protected DateRangePicker Picker;

	public void OnRangeSelect(DateRange range)
	{
		Dispatcher!.Dispatch(new SpecialEvents_SetDateRange_Action(range.Start, range.End));
	}

	void RedirectToLoginClick(string returnUrl)
	{
		NavigationManager!.NavigateTo($"{LoginLink.Login}?returnUrl={returnUrl}", true);
	}
}
