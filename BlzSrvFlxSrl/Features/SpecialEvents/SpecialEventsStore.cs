using static BlzSrvFlxSrl.Data.SqlServer;
using BlzSrvFlxSrl.Features.SpecialEvents.Data;
using Microsoft.AspNetCore.Http;
using BlzSrvFlxSrl.Features.SpecialEvents.Models;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

// 1. Action
public record SpecialEvents_GetListWithDates_Action(DateTimeOffset? DateBegin, DateTimeOffset? DateEnd);

public record SpecialEvents_GetListSuccess_Action(List<SpecialEvent> SpecialEvents); //, string SuccessMessage, int RowCnt
public record SpecialEvents_GetListFailure_Action(string ErrorMessage);
public record SpecialEvents_GetListWarning_Action(string WarningMessage); // Empty List

public record SpecialEvents_Get_Action(int Id, Enums.CommandState? CommandState);
public record SpecialEvents_GetSuccess_Action(FormVM? Model, Enums.CommandState? CommandState);
public record SpecialEvents_GetFailure_Action(string ErrorMessage, bool Warning);

public record SpecialEvents_Submit_Action(FormVM FormVM, Enums.CommandState? CommandState);
public record SpecialEvents_SubmitWithDates_Action(FormVM FormVM, Enums.CommandState? CommandState, DateTimeOffset? DateBegin, DateTimeOffset? DateEnd);
public record SpecialEvents_SubmitSuccess_Action(string SuccessMessage);
public record SpecialEvents_SubmitFailure_Action(string ErrorMessage);

public record SpecialEvents_SetDateRange_Action(DateTimeOffset DateBegin, DateTimeOffset DateEnd);
public record SpecialEvents_ShowForm_Action(bool IsVisible);

public record SpecialEvents_Add_Action();
public record SpecialEvents_Edit_Action(int Id);

public record SpecialEvents_Display_Action(int Id);

public record SpecialEvents_Delete_Action(int Id);
public record SpecialEvents_DeleteSuccess_Action(string SuccessMessage);  
public record SpecialEvents_DeleteFailure_Action(string ErrorMessage);

// 2. State
public record SpecialEventsState
{
	public DateTimeOffset? DateBegin { get; init; }
	public DateTimeOffset? DateEnd { get; init; }
	public Enums.CommandState? CommandState { get; init; }
	public int CurrentId { get; init; }
	public string? FormTitle { get; init; }
	public string? FormSubmitButton { get; init; }
	public bool IsTableVisible { get; init; }
	public bool IsFormVisible { get; init; }
	public bool IsDisplayVisible { get; init; }
	public string? SuccessMessage { get; init; }
	public string? WarningMessage { get; init; }
	public string? ErrorMessage { get; init; }
	public FormVM? Model { get; init; }
	public List<SpecialEvent>? SpecialEventList { get; init; }
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
			DateBegin = DateTime.Parse("3/1/2021"),
			DateEnd = DateTime.Parse("1/21/2023"),
			IsTableVisible = false,
			IsFormVisible = false,
			IsDisplayVisible = false,
			CommandState = Enums.CommandState.Add,  // Should there be a None CommandState?
			CurrentId = 0,
			FormTitle = "Add",
			FormSubmitButton = "Add",
			SuccessMessage = string.Empty,
			WarningMessage = string.Empty,
			ErrorMessage = string.Empty,
			Model = new FormVM(),
			SpecialEventList = new List<SpecialEvent>()
		};
	}
}

// 4. Reducers
public static class SpecialEventsReducers
{
	[ReducerMethod]
	public static SpecialEventsState OnGetListSuccess(
		SpecialEventsState state, SpecialEvents_GetListSuccess_Action action)
	{
		return state with
		{
			IsTableVisible = true,
			WarningMessage = string.Empty,
			ErrorMessage = string.Empty,
			SpecialEventList = action.SpecialEvents
		};
	}

	[ReducerMethod]
	public static SpecialEventsState OnGetListWarning(
		SpecialEventsState state, SpecialEvents_GetListWarning_Action action)
	{
		return state with { WarningMessage = action.WarningMessage };
	}

	[ReducerMethod]
	public static SpecialEventsState OnGetListFailure(
		SpecialEventsState state, SpecialEvents_GetListFailure_Action action)
	{
		return state with { ErrorMessage = action.ErrorMessage };
	}


	[ReducerMethod]
	public static SpecialEventsState OnGet(SpecialEventsState state, SpecialEvents_Get_Action action)
	{
		return state with { CommandState = action.CommandState };
	}

	[ReducerMethod]
	public static SpecialEventsState OnGetSuccess(
		SpecialEventsState state, SpecialEvents_GetSuccess_Action action)
	{
		return state with
		{
			IsFormVisible = action.CommandState == Enums.CommandState.Add || action.CommandState == Enums.CommandState.Edit,
			IsDisplayVisible = action.CommandState == Enums.CommandState.Display,
			Model = action.Model
		};
	}

	[ReducerMethod]
	public static SpecialEventsState OnGetFailure(
			SpecialEventsState state, SpecialEvents_GetFailure_Action action)
	{
		return state with { ErrorMessage = action.ErrorMessage, IsFormVisible = false };
	}


	[ReducerMethod]
	public static SpecialEventsState OnSubmit(SpecialEventsState state, SpecialEvents_Submit_Action action)
	{
		return state with { CommandState = action.CommandState };
	}

	[ReducerMethod]
	public static SpecialEventsState OnSubmitSuccess(
			SpecialEventsState state, SpecialEvents_SubmitSuccess_Action action)
	{
		return state with { IsFormVisible = false, SuccessMessage = "" };
	}

	[ReducerMethod]
	public static SpecialEventsState OnSubmitFailure(
			SpecialEventsState state, SpecialEvents_SubmitFailure_Action action)
	{
		return state with { ErrorMessage = action.ErrorMessage, IsFormVisible = false };
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
		return state with { CurrentId = 0, CommandState = Enums.CommandState.Add, FormTitle = "Add"
			, FormSubmitButton = "Add Event", IsFormVisible = true, IsDisplayVisible = false, Model = new FormVM() };
	}

	[ReducerMethod]
	public static SpecialEventsState OnEdit(
		SpecialEventsState state, SpecialEvents_Edit_Action action)
	{
		return state with { CurrentId = action.Id, CommandState = Enums.CommandState.Edit, FormTitle = "Edit", FormSubmitButton = "Update Event", IsFormVisible = true, IsDisplayVisible = false };
	}

	[ReducerMethod]
	public static SpecialEventsState OnDisplay(
		SpecialEventsState state, SpecialEvents_Display_Action action)
	{
		return state with { CurrentId = action.Id, CommandState = Enums.CommandState.Display, FormTitle = "Display", FormSubmitButton = "", IsFormVisible = false, IsDisplayVisible = true };
	}

	[ReducerMethod]
	public static SpecialEventsState OnDelete(
		SpecialEventsState state, SpecialEvents_Delete_Action action)
	{
		return state with { CurrentId = action.Id, CommandState = Enums.CommandState.Delete, FormTitle = "Delete", FormSubmitButton = "", IsFormVisible = false, IsDisplayVisible = false };
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
	public async Task GetListSpecialEvents(SpecialEvents_GetListWithDates_Action action, IDispatcher dispatcher)
	{
		string inside = nameof(SpecialEventsEffects) + "!" + nameof(GetListSpecialEvents) + "!" + nameof(SpecialEvents_GetListWithDates_Action);

		Logger.LogDebug(string.Format("Inside {0}; Date Range:{1} to {2}", inside, action.DateBegin, action.DateEnd));
		try
		{
			List<Models.SpecialEvent> specialEvents = new();
			specialEvents = await db!.GetEventsByDateRange(action.DateBegin, action.DateEnd);

			if (specialEvents is not null)
			{
				dispatcher.Dispatch(new SpecialEvents_GetListSuccess_Action(specialEvents));
			}
			else
			{
				Logger.LogWarning(string.Format("...{0}; {1} is null", inside, nameof(specialEvents)));
				dispatcher.Dispatch(new SpecialEvents_GetListWarning_Action("No Special Events Found"));
			}

		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
			dispatcher.Dispatch(new SpecialEvents_GetListFailure_Action("An invalid operation occurred, contact your administrator."));
		}
	}


	[EffectMethod]
	public async Task SubmitSpecialEvents(SpecialEvents_Submit_Action action, IDispatcher dispatcher)
	{
		string msgAddOrEdit = string.Empty;
		if (action.CommandState == Enums.CommandState.Add)
		{
			msgAddOrEdit = "Add";
			Logger.LogDebug(string.Format("Inside {0}; Action: {1}"
				, nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents), msgAddOrEdit));
			try
			{
				var sprocTuple = await db.CreateSpecialEvent(action.FormVM);

				/*
				SEE NOTES ON SpecialEventsRepository
				
				if (sprocTuple.NewId != 0)
				{
					dispatcher.Dispatch(new SpecialEvents_SubmitSuccess_Action($"Special Event Added id: [{sprocTuple.NewId}]"));
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
				*/

				dispatcher.Dispatch(new SpecialEvents_SubmitSuccess_Action("Special Event Added id: WHAT IS IT, HAVEN'T GOT A CLUE"));

			}
			catch (Exception ex)
			{
				Logger.LogError(ex, string.Format("...Inside catch of {0}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents)));
				dispatcher.Dispatch(new SpecialEvents_SubmitFailure_Action($"An invalid operation occurred, contact your administrator. Action:{msgAddOrEdit}"));
			}
		}
		else
		{
			msgAddOrEdit = "Edit";
			Logger.LogDebug(string.Format("Inside {0}; Action: {1}; Id: {2}"
				, nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents), msgAddOrEdit, action.FormVM.Id));
			try
			{
				var sprocTuple = await db.UpdateSpecialEvent(action.FormVM);
				dispatcher.Dispatch(new SpecialEvents_SubmitSuccess_Action($"Special Event Updated id: [{action.FormVM.Id}], Affected Rows: {sprocTuple.Affectedrows}"));
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, string.Format("...Inside catch of {0}", nameof(SpecialEventsEffects) + "!" + nameof(SubmitSpecialEvents)));
				dispatcher.Dispatch(new SpecialEvents_SubmitFailure_Action($"An invalid operation occurred, contact your administrator. Action:{msgAddOrEdit}"));
			}
		}

	}


	[EffectMethod]
	public async Task GetSpecialEvents(SpecialEvents_Get_Action action, IDispatcher dispatcher)
	{
		string inside = nameof(SpecialEventsEffects) + "!" + nameof(GetSpecialEvents);

		Logger.LogDebug(string.Format("Inside {0}; id:{1}; CommandState: {2}", inside, action.Id, action.CommandState!.Name));
		try
		{
			Models.SpecialEvent? specialEvent = new();
			specialEvent = await db!.GetEventById(action.Id); // test with 0 

			if (specialEvent is null)
			{
				Logger.LogWarning(string.Format("...{0}; {1} is null", inside, nameof(specialEvent)));
				dispatcher.Dispatch(new SpecialEvents_GetFailure_Action($"Special Event Not Found; Id: {action.Id}", Warning: true));
			}
			else
			{
				Logger.LogDebug(string.Format("...Title: {0}", specialEvent!.Title));
				FormVM formVM = new FormVM
				{
					Id = specialEvent.Id,
					ShowBeginDate = specialEvent.ShowBeginDate,
					ShowEndDate = specialEvent.ShowEndDate,
					EventDate = specialEvent.EventDate,
					SpecialEventTypeId = specialEvent.SpecialEventTypeId,
					Title = specialEvent.Title,
					SubTitle = specialEvent.SubTitle,
					ImageUrl = specialEvent.ImageUrl,
					YouTubeId = specialEvent.YouTubeId,
					WebsiteUrl = specialEvent.WebsiteUrl,
					WebsiteDescr = specialEvent.WebsiteDescr,
					Description = specialEvent.Description
				};

				dispatcher.Dispatch(new SpecialEvents_GetSuccess_Action(formVM, action.CommandState));
				if (action.CommandState == Enums.CommandState.Edit)
				{
					dispatcher.Dispatch(new SpecialEvents_Edit_Action(action.Id));  
				}
				else
				{
					dispatcher.Dispatch(new SpecialEvents_Display_Action(action.Id));
				}
			}

		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
			dispatcher.Dispatch(new SpecialEvents_GetFailure_Action("An invalid operation occurred, contact your administrator", Warning: false));
		}
	}


	[EffectMethod]
	public async Task DeleteSpecialEvents(SpecialEvents_Delete_Action action, IDispatcher dispatcher)
	{
		string inside = nameof(SpecialEventsEffects) + "!" + nameof(DeleteSpecialEvents);
		Logger.LogDebug(string.Format("Inside {0}; Id: {1}", inside, action.Id));
		try
		{
			var affectedrows = await db.RemoveSpecialEvent(action.Id);
			dispatcher.Dispatch(new SpecialEvents_DeleteSuccess_Action($"Special Event {action.Id} has been deleted")); 
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
			dispatcher.Dispatch(new SpecialEvents_DeleteFailure_Action($"An invalid operation occurred, contact your administrator"));
		}
	}

}