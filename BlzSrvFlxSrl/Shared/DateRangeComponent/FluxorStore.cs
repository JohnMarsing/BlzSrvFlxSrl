using Blazored.LocalStorage;
using System;
using System.Reflection;

namespace BlzSrvFlxSrl.Shared.DateRangeComponent;


// 1. Action


/*
If you were persisting lists of State...

public record SetDateRanges_Action(State[] Ranges);
public record LoadDateRanges_Action();
public record LoadDateRangesSuccess_Action();

*/

// ToDo: Rename DateRange with State
public record SetDateRange_Action(DateTimeOffset? BegDate, DateTimeOffset? EndDate);  //ComponentVM ComponentVM
public record SetDateRange2_Action(State State);
public record LoadDateRange_Action();
public record LoadDateRangeSuccess_Action();
public record LoadDateRangeFailure_Action(string ErrorMessage);

public record PersistDateRange_Action(State State);
public record PersistDateRangeSuccess_Action();
public record PersistDateRangeFailure_Action(string ErrorMessage);

public record ClearDateRange_Action();
public record ClearDateRangeSuccess_Action();
public record ClearDateRangeFailure_Action(string ErrorMessage);

// 2. State
public record State
{
	public bool Initialized { get; init; }
	public bool Loading { get; init; }
	public ComponentVM? Model { get; set; }

	//public DateTimeOffset? DateBegin { get; init; }
	//public DateTimeOffset? DateEnd { get; init; }
	//	public string? SuccessMessage { get; init; }
	//	public string? WarningMessage { get; init; }
	//	public string? ErrorMessage { get; init; }
	//public DateRange[] DateRanges { get; init; }
}

// 3. Feature  
public class FeatureImplementation : Feature<State>
{
	public override string GetName() => "DateRange";

	protected override State GetInitialState()
	{
		return new State
		{
			Initialized = false,
			Loading = false,
			Model = new ComponentVM()
			//			DateBegin = DateTime.Parse("3/1/2021"), DateEnd = DateTime.Parse("1/21/2023")
			//DateBegin = DateTime.UtcNow.AddMonths(-6),
			//DateEnd = DateTime.UtcNow.AddMonths(+6)
			//SuccessMessage = string.Empty,
			//WarningMessage = string.Empty,
			//ErrorMessage = string.Empty
		};
	}
}

// 4. Reducers
/*
public static class Reducers
{
	[ReducerMethod]
	public static State OnSetDateRange(State state, SetDateRange_Action action)
	{
		return state with
		{ 
			Model = new ComponentVM(  DateBegin=action.BegDate, DateEnd=action.EndDate ) 
		};
		//return state with { Model = action.ComponentVM };
	}
}
*/

// 5. Effects
public class Effects
{
	private const string DateRangePersistenceName = "BlzSrvFlxSrl_DateRange";

	#region Constructor and DI
	private readonly ILogger Logger;
	private readonly ILocalStorageService _localStorageService;

	public Effects(ILogger<Effects> logger, ILocalStorageService localStorageService)
	{
		Logger = logger;
		_localStorageService = localStorageService;
	}
	#endregion

	[EffectMethod(typeof(LoadDateRange_Action))]
	public async Task LoadDateRange(IDispatcher dispatcher)
	{
		string inside = "namespace:[" + nameof(DateRangeComponent) + "]!" + nameof(Effects) + "!" + nameof(LoadDateRange_Action);
		Logger.LogDebug(string.Format("Inside {0}", inside));

		var daterange = await _localStorageService!.GetItemAsync<ComponentVM>("ComponentVM");
		dispatcher.Dispatch(new SetDateRange_Action(daterange!.DateBegin, daterange!.DateEnd)); // dispatcher.Dispatch(new SetDateRange_Action(daterange));
		dispatcher.Dispatch(new LoadDateRange_Action());
	}

	[EffectMethod]
	public async Task PersistState(PersistDateRange_Action action, IDispatcher dispatcher)
	{
		string inside = nameof(DateRangeComponent) + "!" + nameof(Effects) + "!" + nameof(PersistState) + "!" + nameof(PersistDateRange_Action);
		Logger.LogDebug(string.Format("Inside {0}; Initialized:{1}; Loading:{2}"
				, inside, action.State.Initialized, action.State.Loading));

		if (action.State!.Model!.DateBegin is not null)
		{
			try
			{
				await _localStorageService.SetItemAsync(DateRangePersistenceName, action.State.Model.DateBegin);
				//dispatcher.Dispatch(new PersistDateRangeSuccess_Action());
				Logger.LogDebug(string.Format("...PersistDateRangeSuccess_Action"));
			}
			catch (Exception ex)
			{
				//dispatcher.Dispatch(new PersistDateRangeFailure_Action(ex.Message));
				Logger.LogDebug(string.Format("...PersistDateRangeFailure_Action"));
				dispatcher.Dispatch(new PersistDateRangeFailure_Action(ex.Message));
			}
		}
		else
		{
			Logger.LogDebug(string.Format("...action.State.Model.DateBegin IS null"));
		}


	}

	[EffectMethod(typeof(LoadDateRange_Action))]
	public async Task LoadState(IDispatcher dispatcher)
	{
		try
		{
			var state = await _localStorageService.GetItemAsync<State>(DateRangePersistenceName);
			if (state is not null)
			{
				//dispatcher.Dispatch(new SetDateRange2_Action(state!.Model!));
				dispatcher.Dispatch(new LoadDateRangeSuccess_Action());
			}
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new LoadDateRangeFailure_Action(ex.Message));
		}
	}

	[EffectMethod(typeof(ClearDateRange_Action))]
	public async Task ClearState(IDispatcher dispatcher)
	{
		try
		{
			await _localStorageService.RemoveItemAsync(DateRangePersistenceName);

			/*
			var vm = new ComponentVM
			{
				DateBegin = DateTime.UtcNow.AddMonths(-6),
				DateEnd = DateTime.UtcNow.AddMonths(+6)
			};
			Model = vm
			*/
			
			dispatcher.Dispatch(new SetDateRange2_Action(new State
			{
				Initialized = false,
				Loading = false,
				Model = new ComponentVM()
			}));



			// Used by <Toaster> SubscribeToAction --> WeatherState cleared!
			dispatcher.Dispatch(new ClearDateRangeSuccess_Action());
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new ClearDateRangeFailure_Action(ex.Message));
		}
	}

}
