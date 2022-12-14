using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Form
{
	[Inject] public ILogger<Form>? Logger { get; set; }
	[Inject] private IState<State>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	private Enums.AddEditDisplay _addEditDisplay => SpecialEventsState!.Value.AddEditDisplay!;

	private FormVM? VM => SpecialEventsState!.Value.Model;
	protected void HandleValidSubmit()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Form) + "!" + nameof(HandleValidSubmit)));
		Dispatcher!.Dispatch(new Submit_Action(SpecialEventsState!.Value.Model!, _addEditDisplay));
		Dispatcher?.Dispatch(new GetListWithDates_Action(
			SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
	}

	void CancelActionHandler()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Form) + "!" + nameof(CancelActionHandler)));
		Dispatcher?.Dispatch(new Cancel_Action());
	}

} 

