using Microsoft.AspNetCore.Components;

using static BlzSrvFlxSrl.Features.SqlServer;
using BlzSrvFlxSrl.Features.SpecialEvents.Data;
using Blazored.Toast.Services;

//using System;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;


namespace BlzSrvFlxSrl.Features.SpecialEvents.Stores;

// 1. Action
public record SpecialEventsSubmitAction(FormVM FormVM);
public record SpecialEventsSubmitSuccessAction();
public record SpecialEventsSubmitFailureAction(string ErrorMessage);

// 2. State
public record SpecialEventsState
{
	public DateRange? DateRange { get; init; }
	public int CurrentId { get; init; }
	public bool Submitting { get; init; }
	public bool Submitted { get; init; }
	public string? ErrorMessage { get; init; }
	public FormVM? Model { get; init; }
}

// 3. Feature
public class SpecialEventsStateFeature : Feature<SpecialEventsState>
{
	public override string GetName() => "SpecialEvents";

	protected override SpecialEventsState GetInitialState()
	{
		return new SpecialEventsState
		{
			DateRange = new DateRange
			{
				DateBegin = DateTime.Parse("9/22/2021"),
				DateEnd = DateTime.Parse("1/21/2023")
			},
			CurrentId = 0,
			Submitting = false,
			Submitted = false,
			ErrorMessage = string.Empty,
			Model = new FormVM()
		};
	}
}

// 4. Reducers
public static class SpecialEventsReducers
{
	[ReducerMethod(typeof(SpecialEventsSubmitAction))]
	public static SpecialEventsState OnSubmit(SpecialEventsState state)
	{
		return state with { Submitting = true };
	}

	[ReducerMethod(typeof(SpecialEventsSubmitSuccessAction))]
	public static SpecialEventsState OnSubmitSuccess(SpecialEventsState state)
	{
		return state with { Submitting = false, Submitted = true };
	}

	[ReducerMethod]
	public static SpecialEventsState OnSubmitFailure(
		SpecialEventsState state, SpecialEventsSubmitFailureAction action)
	{
		return state with { Submitting = false, ErrorMessage = action.ErrorMessage };
	}
}



// 5. Effects 
public class SpecialEventsEffects
{
	#region Constructor and DI
	private readonly ILogger Logger;
	private readonly ISpecialEventsRepository db;

	public SpecialEventsEffects(ILogger<SpecialEventsEffects> logger,	ISpecialEventsRepository specialEventsRepository)
	{
		Logger = logger;
		db = specialEventsRepository;
	}
	#endregion

	[EffectMethod]
	public async Task SubmitSpecialEvents(SpecialEventsSubmitAction action, IDispatcher dispatcher)
	{
		Logger.LogDebug(string.Format("Inside {0}, Action: {1}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents), action));
		await Task.Delay(100); // just so we can see the "submitting" message
		try
		{
			var sprocTuple = await db.CreateSpecialEvent(action.FormVM);
			if (sprocTuple.NewId != 0)
			{
				dispatcher.Dispatch(new SpecialEventsSubmitSuccessAction());  // sprocTuple.ReturnMsg
			}
			else
			{
				if (sprocTuple.SprocReturnValue == ReturnValueViolationInUniqueIndex)
				{
					dispatcher.Dispatch(new SpecialEventsSubmitFailureAction(sprocTuple.ReturnMsg + ", [ViolationIn Unique Index]"));
				}
				else
				{
					dispatcher.Dispatch(new SpecialEventsSubmitFailureAction(sprocTuple.ReturnMsg));
				}
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents)));
			dispatcher.Dispatch(new SpecialEventsSubmitFailureAction("An invalid operation occurred, contact your administrator. [Inside catch]"));
		}
	}
}