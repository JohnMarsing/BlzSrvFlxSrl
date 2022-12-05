using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Form
{
	[Inject] public ILogger<Form>? Logger { get; set; }
	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	//private string Title = "Add Upcoming Event";
	private Enums.CommandState _commandState => SpecialEventsState!.Value.CommandState!;

	private FormVM? VM => SpecialEventsState!.Value.Model;
	protected void HandleValidSubmit()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Form) + "!" + nameof(HandleValidSubmit)));
		Dispatcher!.Dispatch(new SpecialEvents_Submit_Action(SpecialEventsState!.Value.Model!, _commandState));
	}

	private void OnInvalidSubmit()
	{
		//Toast.ShowWarning("Invalid Submit");
	}

} 

