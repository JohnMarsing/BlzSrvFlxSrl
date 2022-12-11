using Microsoft.AspNetCore.Components;
using Blazored.Modal.Services;
using BlzSrvFlxSrl.Shared;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class Table
{
	[Inject] public ILogger<Table>? Logger { get; set; }
	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	[CascadingParameter] IModalService Modal { get; set; } = default!;
	
	void AddActionHandler()
	{
		Dispatcher?.Dispatch(new SpecialEvents_Add_Action());
	}

	void PopulateActionHandler()
	{
		Logger!.LogDebug(string.Format("...{0}; Date Range: {1} to {2}"
			, nameof(Table) + "!" + nameof(PopulateActionHandler), SpecialEventsState!.Value.DateBegin, SpecialEventsState!.Value.DateEnd));
		Dispatcher?.Dispatch(new SpecialEvents_GetListWithDates_Action(
			SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
	}

	void EditActionHandler(int id)
	{
		Dispatcher?.Dispatch(new SpecialEvents_Get_Action(id, Enums.CommandState.Edit));
	}

	void DisplayActionHandler(int id)
	{
		Dispatcher?.Dispatch(new SpecialEvents_Get_Action(id, Enums.CommandState.Display));
	}

	private async Task DeleteConfirmationHandler(int id, string title)
	{
		Logger!.LogDebug(string.Format("...{0}; id:{1}, title: {2}", nameof(Table) + "!" + nameof(DeleteConfirmationHandler), id, title));
		var parameters = new ModalParameters { { nameof(ConfirmDeleteModal.Message)
				, $"Special Event {title}" } };
		var modal = Modal.Show<ConfirmDeleteModal>("Confirmation Required", parameters);
		var result = await modal.Result;
		if (result.Confirmed)
		{
			Dispatcher?.Dispatch(new SpecialEvents_Delete_Action(id));
			Dispatcher?.Dispatch(new SpecialEvents_GetListWithDates_Action(
				SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
		}
	}

}
