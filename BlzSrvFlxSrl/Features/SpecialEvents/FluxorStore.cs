// Ignore Spelling: sproc

using static BlzSrvFlxSrl.Data.SqlServer;
using BlzSrvFlxSrl.Features.SpecialEvents.Data;
using Microsoft.AspNetCore.Http;
using BlzSrvFlxSrl.Features.SpecialEvents.Models;
using System.Drawing;

namespace BlzSrvFlxSrl.Features.SpecialEvents;


// 1. Action

// 1.2 Actions related to a list of Items (Id)
public record Get_List_Action(DateTimeOffset? DateBegin, DateTimeOffset? DateEnd);
/*
[EffectMethod] GetList
MasterList!OnInitialized if (SpecialEventsState!.Value.SpecialEventList is null) i.e.first time
MasterList!PopulateActionHandler
MasterList!DeleteConfirmationHandler
Form!HandleValidSubmit after Submit_Action
*/

public record Get_List_Success_Action(List<SpecialEvent> SpecialEvents);  // [EffectMethod] GetList after db!.GetEventsByDateRange and if specialEvents is NOT null
public record Get_List_Warning_Action(string WarningMessage);             // [EffectMethod] GetList after db!.GetEventsByDateRange and if specialEvents IS null
public record Get_List_Failure_Action(string ErrorMessage);               // [EffectMethod] GetList catch (Exception)

// 1.2 Actions related to a single Item
public record Get_Item_Action(int Id, Enums.FormMode? FormMode);                // [EffectMethod] GetItem(Get_Action action,); MasterList!EditActionHandler(int id) or MasterList!DisplayActionHandler(int id) 
public record Get_Item_Success_Action(FormVM? FormVM); // [EffectMethod] GetItem(Get_Action action,); after db!.GetEventById and if specialEvent is NOT null
public record Get_Item_Warning_Action(string WarningMessage);                 // [EffectMethod] GetItem(Get_Action action,); after db!.GetEventById and if specialEvent IS null
public record Get_Item_Failure_Action(string ErrorMessage);                   // [EffectMethod] GetItem catch (Exception)

// 1.3 Actions related to the Form
public record Submitting_Request_Action(FormVM FormVM, Enums.FormMode? FormMode);  // Form!HandleValidSubmit
public record Submitted_Response_Success_Action(string SuccessMessage); // [EffectMethod] Submit: Happy path if action.FormMode == Add or Edit
public record Submitted_Response_Failure_Action(string ErrorMessage);   // [EffectMethod] Submit: Sad path (catch Exception)  if action.FormMode == Add or Edit

// 1.4 Actions related to the MasterList (specifically the ActionButtons!EventCallback)
public record Add_Action();           // Dispatched by MasterList!AddActionHandler
public record Edit_Action(int Id);    // [EffectMethod] GetItem if the happy path occurred and FormMode == FormMode.Edi

public record Display_Action(int Id); // [EffectMethod] GetItem if the happy path occurred and FormMode != FormMode.Edi
public record Delete_Action(int Id);
// [EffectMethod] Delete
// [EffectMethod] MasterList!DeleteConfirmationHandler and IsModalConfirmed is true

public record DeleteSuccess_Action(string SuccessMessage);   // [EffectMethod] Delete Happy path
public record DeleteFailure_Action(string ErrorMessage);    // [EffectMethod] Delete sad path

public record Set_PageHeader_For_Index_Action(PageHeaderVM PageHeaderVM);
public record Set_PageHeader_For_Detail_Action(string Title, string Icon, string Color, int Id);


// 2. State
public record State
{
	public DateTimeOffset? DateBegin { get; init; }
	public DateTimeOffset? DateEnd { get; init; }
	public Enums.VisibleComponent? VisibleComponent { get; init; }
	public Enums.FormMode? FormMode { get; init; }
	public string? SuccessMessage { get; init; }
	public string? WarningMessage { get; init; }
	public string? ErrorMessage { get; init; }
	public FormVM? FormVM { get; init; }
	public List<SpecialEvent>? SpecialEventList { get; init; }
	public PageHeaderVM? PageHeaderVM { get; init; }
}

// 3. Feature  
public class FeatureImplementation : Feature<State>
{
	public override string GetName() => "SpecialEvents";

	protected override State GetInitialState()
	{
		return new State
		{
			//  ToDo: can't used these random default dates
			DateBegin = DateTime.Parse("3/1/2021"),
			DateEnd = DateTime.Parse("6/21/2023"),
			FormMode = null,
			VisibleComponent = Enums.VisibleComponent.MasterList,
			SuccessMessage = string.Empty,
			WarningMessage = string.Empty,
			ErrorMessage = string.Empty,
			PageHeaderVM = Constants.GetPageHeaderForIndexVM(),
			FormVM = new FormVM()
		};
	}
}

// 4. Reducers
public static class Reducers
{

	[ReducerMethod]
	public static State On_Get_List_Success(
		State state, Get_List_Success_Action action)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.MasterList,
			WarningMessage = string.Empty,
			ErrorMessage = string.Empty,
			SpecialEventList = action.SpecialEvents
		};
	}

	[ReducerMethod]
	public static State On_Get_List_Warning(
		State state, Get_List_Warning_Action action)
	{
		// Here might be a case where VisibleComponent has a .Table and 
		//  You got this warning (no records found) then set it to .None
		//  Therefore, this would be the only place where VisibleComponent = None
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.MasterList,
			WarningMessage = action.WarningMessage
		};
	}

	[ReducerMethod]
	public static State On_Get_List_Failure(
		State state, Get_List_Failure_Action action)
	{
		return state with { ErrorMessage = action.ErrorMessage };
	}

	[ReducerMethod]
	public static State On_Get_Item(State state, Get_Item_Action action)
	{
		return state with { FormMode = action.FormMode };
	}


	[ReducerMethod]
	public static State On_Get_Item_Success(
		State state, Get_Item_Success_Action action)  
	{
		return state with
		{
			FormVM = action.FormVM
		};
	}

	[ReducerMethod]
	public static State On_Get_Item_Failure(
			State state, Get_Item_Failure_Action action)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.MasterList,
			ErrorMessage = action.ErrorMessage
		};
	}

	// Called by Form.HandleValidSubmit; Step 1
	[ReducerMethod]
	public static State On_Submitting_Request(
		State state, Submitting_Request_Action action)
	{
		return state with { FormMode = action.FormMode };
	}

	// ToDo: Unlike `On_Submitted_Response_Failure`, there's no `[ReducerMethod]` ergo there's no `action` parameter
	public static State On_Submitted_Response_Success(State state)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.MasterList,
			SuccessMessage = ""  // ToDo: this is different than `Submitted_Response_Failure_Action`
		};
	}

	[ReducerMethod]
	public static State On_Submitted_Response_Failure(
			State state, Submitted_Response_Failure_Action action)
	{
		return state with
		{
			ErrorMessage = action.ErrorMessage,
			VisibleComponent = Enums.VisibleComponent.MasterList
		};
	}


	[ReducerMethod]
	public static State OnAdd(
		State state, Add_Action action)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.AddEditForm,
			FormMode = Enums.FormMode.Add,
			FormVM = new FormVM()
		};
	}

	[ReducerMethod]
	public static State OnEdit(
		State state, Edit_Action action)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.AddEditForm,
			FormMode = Enums.FormMode.Edit,
		};
	}

	[ReducerMethod]
	public static State OnDisplay(
		State state, Display_Action action)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.DisplayCard
		};
	}

	[ReducerMethod]
	public static State OnDelete(
		State state, Delete_Action action)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.MasterList,
		};
	}

	[ReducerMethod]
	public static State On_Set_PageHeader_For_Index(
	State state, Set_PageHeader_For_Index_Action action)
	{
		return state with
		{
			VisibleComponent = Enums.VisibleComponent.MasterList,
			PageHeaderVM = Constants.GetPageHeaderForIndexVM()
		};
	}


	[ReducerMethod]
	public static State On_Set_PageHeader_For_Detail(
		State state, Set_PageHeader_For_Detail_Action action)
	{
		return state with
		{
			PageHeaderVM = new PageHeaderVM { Title = action.Title, Icon = action.Icon, Color = action.Color, Id = action.Id }
		};
	}

}



// 5. Effects 
public class Effects
{
	#region Constructor and DI
	private readonly ILogger Logger;
	private readonly IRepository db;

	public Effects(ILogger<Effects> logger, IRepository repository)
	{
		Logger = logger;
		db = repository;
	}
	#endregion

	/*
	Dispatched by...

	- IndexForm
		- OnInitialized; 
		- OnRangeSelect; Step 2 (after SetDateRange_Action)

	- Form.HandleValidSubmit; Step 2 (after Submit_Action)

	- Table
		- PopulateActionHandler (ActionButtons.PopulateButton)
		- DeleteConfirmationHandler; Step 2 after Delete_Action

	External call...
	- db!.GetEventsByDateRange(action.DateBegin, action.DateEnd)

	Dispatched to...
	- GetListSuccess_Action if specialEvents is not null
	- GetListWarning_Action if specialEvents is null
	- GetListFailure_Action if db call fails.
	*/

	[EffectMethod]
	public async Task GetList(Get_List_Action action, IDispatcher dispatcher)
	{
		string inside = nameof(Effects) + "!" + nameof(GetList) + "!" + nameof(Get_List_Action);

		Logger.LogDebug(string.Format("Inside {0}; Date Range:{1} to {2}", inside, action.DateBegin, action.DateEnd));
		try
		{
			List<Models.SpecialEvent> specialEvents = new();
			specialEvents = await db!.GetEventsByDateRange(action.DateBegin, action.DateEnd);

			if (specialEvents is not null)
			{
				dispatcher.Dispatch(new Get_List_Success_Action(specialEvents));
			}
			else
			{
				Logger.LogWarning(string.Format("...{0}; {1} is null", inside, nameof(specialEvents)));
				dispatcher.Dispatch(new Get_List_Warning_Action("No Special Events Found"));
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
			dispatcher.Dispatch(new Get_List_Failure_Action("An invalid operation occurred, contact your administrator."));
		}
	}

	// Called by Form.HandleValidSubmit; Step 1
	[EffectMethod]
	public async Task Submit(Submitting_Request_Action action, IDispatcher dispatcher)
	{
		// What's the best way to deal with this
		if (action.FormMode is null) throw new ArgumentException("Parameter cannot be null", nameof(action.FormMode));

		string inside = $"{nameof(Effects)}!{nameof(Submit)}; Action: {action.FormMode.Name}";

		if (action.FormMode == Enums.FormMode.Add)
		{
			Logger.LogDebug(string.Format("Inside {0}", inside));
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

				dispatcher.Dispatch(new Submitted_Response_Success_Action("Special Event Added id: WHAT IS IT...HAVEN'T GOT A CLUE"));

			}
			catch (Exception ex)
			{
				Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
				dispatcher.Dispatch(new Submitted_Response_Failure_Action($"An invalid operation occurred, contact your administrator. Action: {action.FormMode.Name}"));
			}
		}
		else
		{
			Logger.LogDebug(string.Format("Inside {0}; Id: {1}", inside, action.FormVM.Id));
			try
			{
				var sprocTuple = await db.UpdateSpecialEvent(action.FormVM);
				dispatcher.Dispatch(new Submitted_Response_Success_Action(
					$"Special Event Updated for id: [{action.FormVM.Id}], Affected Rows: {sprocTuple.Affectedrows}"));
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
				dispatcher.Dispatch(new Submitted_Response_Failure_Action(
					$"An invalid operation occurred, contact your administrator. Action: {action.FormMode.Name}"));
			}
		}

	}


	/*
	Dispatched by...
	- Table
		- EditActionHandler (ActionButtons.PopulateButton)
		- DisplayActionHandler; Step 2 after Delete_Action

	External call...
	- db!.GetEventById(action.Id)

	Dispatched to...
	- GetSuccess_Action if specialEvents is not null
		- Edit_Action(action.Id)    if FormMode == Enums.FormMode.Edit
		- Display_Action(action.Id) if FormMode == Enums.FormMode.Display

	- GetWarning_Action if specialEvents is null

	- GetFailure_Action if db call fails.
*/
	[EffectMethod]
	public async Task GetItem(Get_Item_Action action, IDispatcher dispatcher)
	{
		string inside = $"{nameof(Effects)}!{nameof(GetItem)};  Action: {nameof(action.FormMode.Name)}; Id: {action.Id}";

		Logger.LogDebug(string.Format("Inside {0}", inside));
		try
		{
			Models.SpecialEvent? specialEvent = new();
			specialEvent = await db!.GetEventById(action.Id); // test with 0 

			if (specialEvent is null)
			{
				Logger.LogWarning(string.Format("...{0}; {1} is null", inside, nameof(specialEvent)));
				dispatcher.Dispatch(new Get_Item_Warning_Action($"Special Event Not Found; Id: {action.Id}"));
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

				dispatcher.Dispatch(new Get_Item_Success_Action(formVM));

				if (action.FormMode == Enums.FormMode.Edit)
				{
					dispatcher.Dispatch(new Edit_Action(action.Id));
				}
				else
				{
					dispatcher.Dispatch(new Display_Action(action.Id));
				}
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
			dispatcher.Dispatch(new Get_Item_Failure_Action("An invalid operation occurred, contact your administrator"));
		}
	}

	[EffectMethod]
	public async Task Delete(Delete_Action action, IDispatcher dispatcher)
	{
		string inside = $"{nameof(Effects)}!{nameof(Delete)}; Id: {action.Id}";
		Logger.LogDebug(string.Format("Inside {0}; Id: {1}", inside, action.Id));
		try
		{
			var affectedRows = await db.RemoveSpecialEvent(action.Id);
			dispatcher.Dispatch(new DeleteSuccess_Action($"Special Event {action.Id} has been deleted"));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, string.Format("...Inside catch of {0}", inside));
			dispatcher.Dispatch(new DeleteFailure_Action($"An invalid operation occurred, contact your administrator"));
		}
	}

}