using Microsoft.AspNetCore.Components;
using BlazorDateRangePicker;

namespace BlzSrvFlxSrl.Features.SpecialEvents.DateRangeComponent;

public partial class Component
{
	[Inject] private IState<State>? DateRangeComponentState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }
	[Inject] public ILogger<Component>? Logger { get; set; }

	private DateTimeOffset? DateBegin { get; set; }
	private DateTimeOffset? DateEnd { get; set; }
	

//	protected override void OnInitialized()
//	{
//		Logger!.LogDebug(string.Format("Inside {0}", nameof(Component) + "!" + nameof(OnInitializedAsync)));
//		Dispatcher!.Dispatch(new DateRangeComponent.LoadDateRange_Action());

//		ComponentVM? vm = new ComponentVM();
//		vm= DateRangeComponentState!.Value.Model;
//		Dispatcher!.Dispatch(new GetListWithDates_Action(vm.DateBegin, vm.DateEnd));

///*
//		DateBegin = DateRangeComponentState!.Value!.Model!.DateBegin;
//		DateEnd = DateRangeComponentState!.Value.Model.DateEnd;
//		Dispatcher!.Dispatch(new GetListWithDates_Action(DateBegin, DateEnd));
//*/
//		base.OnInitialized();
//	}

	public void OnRangeSelect(DateRange range)
	{
		/*
		var vm = new ComponentVM
		{
			DateBegin = range.Start,
			DateEnd = range.End
		};
		
		Dispatcher!.Dispatch(new SetDateRange_Action(vm));

		*/


		Dispatcher!.Dispatch(new SetDateRange_Action(range.Start, range.End)); //   vm
		Dispatcher!.Dispatch(new Get_List_Action(
			DateRangeComponentState!.Value.Model!.DateBegin, DateRangeComponentState.Value.Model!.DateEnd));
	}

	//string datesMsg = "null";
	void LogDates()
	{
		if (DateRangeComponentState!.Value!.Model!.DateBegin is not null)
		{
			Logger!.LogDebug(string.Format("Inside {0}", nameof(Component) + "!" + nameof(LogDates)));
			Logger!.LogDebug(string.Format("..Dates: {0} - {1}", DateRangeComponentState!.Value!.Model!.DateBegin, DateRangeComponentState!.Value!.Model!.DateEnd));
		}
		else
		{
			Logger!.LogDebug(string.Format("..Dates still null" ));
		}
	}

	void PersistDates()
	{
		Dispatcher!.Dispatch(new DateRangeComponent.PersistDateRange_Action(DateRangeComponentState!.Value));
	}


	
}