using Microsoft.AspNetCore.Components;
using BlzSrvFlxSrl.Shared;
using Blazored.Modal.Services;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class MasterListCardXsSm
{
	[Inject] public ILogger<MasterListCardXsSm>? Logger { get; set; }
	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	[CascadingParameter] IModalService Modal { get; set; } = default!;

	void AddActionHandler()
	{
		Logger!.LogDebug(string.Format("inside: {0}", nameof(MasterListCardXsSm) + "!" + nameof(AddActionHandler)));
		Dispatcher?.Dispatch(new SpecialEvents_Add_Action());
	}

	void PopulateActionHandler()
	{
		Logger!.LogDebug(string.Format("...{0}; Date Range: {1} to {2}"
			, nameof(MasterListCardXsSm) + "!" + nameof(PopulateActionHandler), SpecialEventsState!.Value.DateBegin, SpecialEventsState!.Value.DateEnd));
		Dispatcher?.Dispatch(new SpecialEvents_GetListWithDates_Action(
			SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
	}

	void EditActionHandler(int id)
	{
		Logger!.LogDebug(string.Format("inside: {0}; id:{1}", nameof(MasterListCardXsSm) + "!" + nameof(EditActionHandler), id));
		Dispatcher?.Dispatch(new SpecialEvents_Get_Action(id, Enums.AddEditDisplay.Edit));
	}

	void DisplayActionHandler(int id)
	{
		Logger!.LogDebug(string.Format("inside: {0}; id:{1}", nameof(MasterListCardXsSm) + "!" + nameof(DisplayActionHandler), id));
		Dispatcher?.Dispatch(new SpecialEvents_Get_Action(id, Enums.AddEditDisplay.Display));
	}

	private async Task DeleteConfirmationHandler(int id, string title)
	{
		Logger!.LogDebug(string.Format("...{0}; id:{1}, title: {2}", nameof(MasterListCardXsSm) + "!" + nameof(DeleteConfirmationHandler), id, title));
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