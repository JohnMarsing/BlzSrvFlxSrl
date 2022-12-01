using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using BlzSrvFlxSrl.Features.SpecialEvents.Enums;
using BlzSrvFlxSrl.Features.SpecialEvents.Data;

using static BlzSrvFlxSrl.Features.SqlServer;
using Blazored.Toast.Services;


namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Form
{
	//[Inject] public ISpecialEventsRepository db { get; set; }
	[Inject] public ILogger<Form> Logger { get; set; }
	//[Inject] public IToastService Toast { get; set; }
	//[Inject] private IState<MainState> MainState { get; set; }
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

