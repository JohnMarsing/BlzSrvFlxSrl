using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Blazored.Toast.Services;
using BlzSrvFlxSrl.Features.SpecialEvents.Models;
using BlzSrvFlxSrl.Features.SpecialEvents.Data;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Table
{
	[Inject] public ISpecialEventsRepository db { get; set; }
	[Inject] public ILogger<Table>? Logger { get; set; }
	[Inject] public IToastService? Toast { get; set; }

	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	private DateTimeOffset? DateBegin { get; set; }
	private DateTimeOffset? DateEnd { get; set; }

	protected List<SpecialEvent>? SpecialEvents;

	protected override async Task OnInitializedAsync()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(Table) + "!" + nameof(OnInitializedAsync) ) );

		DateBegin = SpecialEventsState!.Value.DateBegin;
		DateEnd = SpecialEventsState!.Value.DateEnd;

		if (DateBegin is not null) 
		{
			await PopulateTable();  // DateBegin, DateEnd
		}
		else
		{
			//Toast.ShowInfo($"MainState.Value.DateRange is null");
			Logger!.LogDebug(string.Format("...MainState.Value.DateRange is null, nothing to do"));
		}
		await base.OnInitializedAsync();  // _ = base.OnInitializedAsync();
	}

	// Port to state so that Add/Edit can refresh the table
	protected async Task PopulateTable() //DateTimeOffset? dateBegin, DateTimeOffset? dateEnd
	{
		Logger!.LogDebug(string.Format("Inside {0}, DateBegin:{1}, DateEnd:{2}"
			, nameof(Table) + "!" + nameof(PopulateTable), DateBegin, DateEnd ));
		SpecialEvents = await db.GetEventsByDateRange(DateBegin, DateEnd);  
	}

	void AddActionHandler()
	{
		Dispatcher?.Dispatch(new SpecialEvents_Add_Action());
	}

	void EditActionHandler(int id)
	{
		Dispatcher?.Dispatch(new SpecialEvents_Edit_Action(SpecialEventsState!.Value.CurrentId));
		Dispatcher?.Dispatch(new SpecialEvents_Get_Action(id, Enums.CommandState.Edit));
	}

	void DisplayActionHandler(int id)
	{
		Dispatcher?.Dispatch(new SpecialEvents_Get_Action(id, Enums.CommandState.Display));
	}

	void DeleteActionHandler(int id)
	{
		Logger!.LogDebug(string.Format("...{0}; id:{1}", nameof(Table) + "!" + nameof(DeleteActionHandler), id));
		Dispatcher?.Dispatch(new SpecialEvents_Delete_Action(id));
	}

}
