using static BlzSrvFlxSrl.Features.SqlServer;
using BlzSrvFlxSrl.Features.SpecialEvents.Data;
using Microsoft.AspNetCore.Http;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

// 1. Action
public record SpecialEvents_Get_Action(int Id, Enums.CommandState? CommandState);
public record SpecialEvents_GetSuccess_Action();
public record SpecialEvents_GetFailure_Action(string ErrorMessage);

public record SpecialEvents_Submit_Action(FormVM FormVM, Enums.CommandState? CommandState);
public record SpecialEvents_SubmitSuccess_Action();
public record SpecialEvents_SubmitFailure_Action(string ErrorMessage);

public record SpecialEvents_SetDateRange_Action(DateTimeOffset DateBegin, DateTimeOffset DateEnd);
public record SpecialEvents_ShowForm_Action(bool IsVisible);

public record SpecialEvents_Add_Action();
public record SpecialEvents_Edit_Action(int Id);
public record SpecialEvents_Display_Action(int Id);
public record SpecialEvents_Delete_Action(int Id);

// 2. State
public record SpecialEventsState
{
	public DateTimeOffset? DateBegin { get; init; }
	public DateTimeOffset? DateEnd { get; init; }
	public Enums.CommandState? CommandState { get; init; }
	public int CurrentId { get; init; }
	public string? FormTitle { get; init; }
	public string? FormSubmitButton { get; init; }
	public bool IsFormVisible { get; init; }
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
			IsFormVisible = false,
			CommandState = Enums.CommandState.Add,
			CurrentId = 0,
			FormTitle = "Add",
			FormSubmitButton = "Add",
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

	[ReducerMethod]
	public static SpecialEventsState OnGet(SpecialEventsState state, SpecialEvents_Get_Action action)
	{
		return state with { CommandState = action.CommandState };
	}

	[ReducerMethod(typeof(SpecialEvents_GetSuccess_Action))]
	public static SpecialEventsState OnGetSuccess(SpecialEventsState state)
	{
		return state with { Submitting = false, Submitted = true, IsFormVisible = false };
	}

	[ReducerMethod]
	public static SpecialEventsState OnGetFailure(
			SpecialEventsState state, SpecialEvents_GetFailure_Action action)
	{
		return state with { Submitting = false, ErrorMessage = action.ErrorMessage, IsFormVisible = false };
	}



	//(typeof(SpecialEvents_Submit_Action))]
	[ReducerMethod]
	public static SpecialEventsState OnSubmit(SpecialEventsState state, SpecialEvents_Submit_Action action)
	{
		return state with { Submitting = true, CommandState = action.CommandState };
	}

	[ReducerMethod(typeof(SpecialEvents_SubmitSuccess_Action))]
	public static SpecialEventsState OnSubmitSuccess(SpecialEventsState state)
	{
		return state with { Submitting = false, Submitted = true, IsFormVisible = false };
	}

	[ReducerMethod]
	public static SpecialEventsState OnSubmitFailure(
			SpecialEventsState state, SpecialEvents_SubmitFailure_Action action)
	{
		return state with { Submitting = false, ErrorMessage = action.ErrorMessage, IsFormVisible = false };
	}

	[ReducerMethod]
	public static SpecialEventsState OnSetDateRange(
		SpecialEventsState state, SpecialEvents_SetDateRange_Action action)
	{
		return state with { DateBegin = action.DateBegin, DateEnd = action.DateEnd };
	}


	[ReducerMethod]
	public static SpecialEventsState OnAdd(
		SpecialEventsState state, SpecialEvents_Add_Action action)
	{
		return state with { CurrentId = 0, CommandState = Enums.CommandState.Add, FormTitle = "Add", FormSubmitButton = "Add Event", IsFormVisible = true };
	}

	[ReducerMethod]
	public static SpecialEventsState OnEdit(
		SpecialEventsState state, SpecialEvents_Edit_Action action)
	{
		return state with { CurrentId = action.Id, CommandState = Enums.CommandState.Edit, FormTitle = "Edit", FormSubmitButton = "Update Event", IsFormVisible = true };
	}

	[ReducerMethod]
	public static SpecialEventsState OnDisplay(
		SpecialEventsState state, SpecialEvents_Display_Action action)
	{
		return state with { CurrentId = action.Id, CommandState = Enums.CommandState.Display, FormTitle = "Display", FormSubmitButton = "", IsFormVisible = true };
	}

	[ReducerMethod]
	public static SpecialEventsState OnDelete(
		SpecialEventsState state, SpecialEvents_Delete_Action action)
	{
		return state with { CurrentId = action.Id, CommandState = Enums.CommandState.Delete, FormTitle = "Delete", FormSubmitButton = "", IsFormVisible = true };
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
		if (action.CommandState == Enums.CommandState.Add)
		{
			Logger.LogDebug(string.Format("Inside {0}; Add"
				, nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents), action));
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
		else
		{
			Logger.LogDebug(string.Format("Inside {0}; Edit Id: {1}"
				, nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents), action.FormVM.Id));
			try
			{
				var sprocTuple = await db.UpdateSpecialEvent(action.FormVM);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, string.Format("...Inside catch of {0}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents)));
				dispatcher.Dispatch(new SpecialEvents_SubmitFailure_Action("An invalid operation occurred, contact your administrator. [Inside catch]"));
			}

		}

	}

	/**/

	[EffectMethod]
	public async Task GetSpecialEvents(SpecialEvents_Get_Action action, IDispatcher dispatcher)
	{
		Logger.LogDebug(string.Format("Inside {0}; id:{1}"
			, nameof(SpecialEventsEffects) + "!" + nameof(GetSpecialEvents), action.Id));
		try
		{
			Models.SpecialEvent specialEvent = new Models.SpecialEvent();
			specialEvent = await db.GetEventById(action.Id);
			dispatcher.Dispatch(new SpecialEvents_GetFailure_Action("An invalid operation occurred, contact your administrator. [Inside catch]"));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents)));
			dispatcher.Dispatch(new SpecialEvents_GetFailure_Action("An invalid operation occurred, contact your administrator. [Inside catch]"));
		}
	}
	

}