using Microsoft.AspNetCore.Components;


namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Form
{
	[Inject] public ILogger<Form> Logger { get; set; }
	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }

	private string Title = "Add Upcoming Event";

	private FormVM? VM => SpecialEventsState!.Value.Model;
	protected void HandleValidSubmit()
	{
		Logger.LogDebug(string.Format("Inside {0}, VM.ToString: {1}"
			, nameof(Form) + "!" + nameof(HandleValidSubmit), VM.ToString()));

		Dispatcher.Dispatch(new SpecialEvents_Submit_Action(SpecialEventsState!.Value.Model!));

	}

	private void OnInvalidSubmit()
	{
		//Toast.ShowWarning("Invalid Submit");
	}

} 

