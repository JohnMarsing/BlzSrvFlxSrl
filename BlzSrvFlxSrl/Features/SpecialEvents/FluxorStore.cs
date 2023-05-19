namespace BlzSrvFlxSrl.Features.SpecialEvents;

#region 1. Action
// 1.1 GetList() actions
public record Get_List_Action(DateTimeOffset? DateBegin, DateTimeOffset? DateEnd);
public record Get_List_Success_Action(List<Data.vwSpecialEvent> SpecialEvents);
public record Get_List_Warning_Action(string WarningMessage);
public record Get_List_Failure_Action(string ErrorMessage);

// 1.2 GetItem() actions
public record Get_Item_Action(int Id, Enums.FormMode? FormMode);
public record Get_Item_Success_Action(FormVM? FormVM);
public record Get_Item_Warning_Action(string WarningMessage);
public record Get_Item_Failure_Action(string ErrorMessage);
public record Edit_Action(int Id);
public record Display_Action(int Id);

// 1.3 Actions related to Form Submission
public record Submitting_Request_Action(FormVM FormVM, Enums.FormMode? FormMode);
public record Submitted_Response_Success_Action(string SuccessMessage);
public record Submitted_Response_Failure_Action(string ErrorMessage);

// 1.4 Actions related to MasterList
public record Add_Action();

// 1.5 Delete() actions
public record Delete_Action(int Id);
public record DeleteSuccess_Action(string SuccessMessage);
public record DeleteFailure_Action(string ErrorMessage);

// 1.6 Delete() actions
public record Set_PageHeader_For_Index_Action(PageHeaderVM PageHeaderVM);
public record Set_PageHeader_For_Detail_Action(string Title, string Icon, string Color, int Id);
#endregion

// 2. State
public record State
{
	// See wiki about notes
	public DateTimeOffset? DateBegin { get; init; }
	public DateTimeOffset? DateEnd { get; init; }

	public Enums.VisibleComponent? VisibleComponent { get; init; }
	public Enums.FormMode? FormMode { get; init; }
	public string? SuccessMessage { get; init; }
	public string? WarningMessage { get; init; }
	public string? ErrorMessage { get; init; }
	public FormVM? FormVM { get; init; }
	public List<Data.vwSpecialEvent>? SpecialEventList { get; init; }
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
			DateBegin = DateTime.Parse(Constants.DateRange.Start),
			DateEnd = DateTime.Parse(Constants.DateRange.End),
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
	private readonly Data.IRepository db;

	public Effects(ILogger<Effects> logger, Data.IRepository repository)
	{
		Logger = logger;
		db = repository;
	}
	#endregion

	[EffectMethod]
	public async Task GetList(Get_List_Action action, IDispatcher dispatcher)
	{
		string inside = nameof(Effects) + "!" + nameof(GetList) + "!" + nameof(Get_List_Action);

		Logger.LogDebug(string.Format("Inside {0}; Date Range:{1} to {2}", inside, action.DateBegin, action.DateEnd));
		try
		{
			List<Data.vwSpecialEvent> specialEvents = new();
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

	[EffectMethod]
	public async Task Submit(Submitting_Request_Action action, IDispatcher dispatcher)
	{
		if (action.FormMode is null) throw new ArgumentException("Parameter cannot be null", nameof(action.FormMode));

		string inside = $"{nameof(Effects)}!{nameof(Submit)}; Action: {action.FormMode.Name}";

		if (action.FormMode == Enums.FormMode.Add)
		{
			Logger.LogDebug(string.Format("Inside {0}", inside));
			try
			{
				var sprocTuple = await db.CreateSpecialEvent(action.FormVM);

				dispatcher.Dispatch(new Submitted_Response_Success_Action("Special Event Added id: ???"));  //SEE NOTES ON SpecialEventsRepository

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



	[EffectMethod]
	public async Task GetItem(Get_Item_Action action, IDispatcher dispatcher)
	{
		string inside = $"{nameof(Effects)}!{nameof(GetItem)};  Action: {nameof(action.FormMode.Name)}; Id: {action.Id}";

		Logger.LogDebug(string.Format("Inside {0}", inside));
		try
		{
			Data.vwSpecialEvent? specialEvent = new();
			specialEvent = await db!.GetEventById(action.Id); 

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