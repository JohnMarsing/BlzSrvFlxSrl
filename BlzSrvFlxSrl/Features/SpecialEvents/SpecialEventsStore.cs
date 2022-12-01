using static BlzSrvFlxSrl.Features.SqlServer;
using BlzSrvFlxSrl.Features.SpecialEvents.Data;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

// 1. Action
public record SpecialEvents_Submit_Action(FormVM FormVM);
public record SpecialEvents_SubmitSuccess_Action();
public record SpecialEvents_SubmitFailure_Action(string ErrorMessage);
public record SpecialEvents_SetDateRange_Action(DateTimeOffset DateBegin, DateTimeOffset DateEnd);

// 2. State
public record SpecialEventsState
{
	public DateTimeOffset? DateBegin { get; init; }
	public DateTimeOffset? DateEnd { get; init; }
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
			//  ToDo: can't used these random default dates
			DateBegin = DateTime.Parse("9/22/2021"),
			DateEnd = DateTime.Parse("1/21/2023"),
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
	[ReducerMethod(typeof(SpecialEvents_Submit_Action))]
	public static SpecialEventsState OnSubmit(SpecialEventsState state)
	{
		return state with { Submitting = true };
	}

	[ReducerMethod(typeof(SpecialEvents_SubmitSuccess_Action))]
	public static SpecialEventsState OnSubmitSuccess(SpecialEventsState state)
	{
		return state with { Submitting = false, Submitted = true };
	}

	[ReducerMethod]
	public static SpecialEventsState OnSubmitFailure(
			SpecialEventsState state, SpecialEvents_SubmitFailure_Action action)
	{
		return state with { Submitting = false, ErrorMessage = action.ErrorMessage };
	}

	[ReducerMethod]
	public static SpecialEventsState OnSetDateRange(
		SpecialEventsState state, SpecialEvents_SetDateRange_Action action)
	{
		return state with { DateBegin = action.DateBegin, DateEnd = action.DateEnd };
	}
}



// 5. Effects 
public class SpecialEventsEffects
{
	#region Constructor and DI
	private readonly ILogger Logger;
	private readonly ISpecialEventsRepository db;

	public SpecialEventsEffects(ILogger<SpecialEventsEffects> logger, ISpecialEventsRepository specialEventsRepository)
	{
		Logger = logger;
		db = specialEventsRepository;
	}
	#endregion

	[EffectMethod]
	public async Task SubmitSpecialEvents(SpecialEvents_Submit_Action action, IDispatcher dispatcher)
	{
		Logger.LogDebug(string.Format("Inside {0}, Action: {1}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents), action));
		await Task.Delay(100); // just so we can see the "submitting" message
		try
		{
			var sprocTuple = await db.CreateSpecialEvent(action.FormVM);
			if (sprocTuple.NewId != 0)
			{
				dispatcher.Dispatch(new SpecialEvents_SubmitSuccess_Action());  // sprocTuple.ReturnMsg
			}
			else
			{
				if (sprocTuple.SprocReturnValue == ReturnValueViolationInUniqueIndex)
				{
					dispatcher.Dispatch(new SpecialEvents_SubmitFailure_Action(sprocTuple.ReturnMsg + ", [ViolationIn Unique Index]"));
				}
				else
				{
					dispatcher.Dispatch(new SpecialEvents_SubmitFailure_Action(sprocTuple.ReturnMsg));
				}
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents)));
			dispatcher.Dispatch(new SpecialEvents_SubmitFailure_Action("An invalid operation occurred, contact your administrator. [Inside catch]"));
		}
	}
}