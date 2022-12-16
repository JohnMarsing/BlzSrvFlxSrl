using Microsoft.AspNetCore.Components;
using BlzSrvFlxSrl.Shared;
using Blazored.Modal.Services;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class MasterList
{
	[Inject] public ILogger<MasterList>? Logger { get; set; }
	[Inject] private IState<State>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	[Parameter, EditorRequired] public bool IsXsOrSm { get; set; }

	[CascadingParameter] IModalService Modal { get; set; } = default!;

	private async Task OnCallBackEvent(CallBackEventArgs args)
	{		
		await Task.Delay(0);
		int id = args.Id;

		string crudName;
		if (args.Crud == null)
		{
			Logger!.LogWarning(string.Format("inside: {0}; args.Crud: == null", nameof(MasterList) + "!" + nameof(OnCallBackEvent)));
			crudName = "Display";
		}
		else
		{
			crudName = args.Crud.Name;
		}

		switch (crudName)
		{
			case "Add":
				AddActionHandler();
				break;

			case "Edit":
				EditActionHandler(id);
				break;

			case "Display":
				DisplayActionHandler(id);
				break;

			case "Delete":
				DeleteConfirmationHandler(id, "title");
				break;

			case "Repopulate":
				PopulateActionHandler();
				break;

			default:
				break;
		}
	}

	void AddActionHandler()
	{
		Logger!.LogDebug(string.Format("inside: {0}", nameof(MasterList) + "!" + nameof(AddActionHandler)));
		Dispatcher?.Dispatch(new Add_Action());
	}

	void PopulateActionHandler()
	{
		Logger!.LogDebug(string.Format("...{0}; Date Range: {1} to {2}"
			, nameof(MasterList) + "!" + nameof(PopulateActionHandler), SpecialEventsState!.Value.DateBegin, SpecialEventsState!.Value.DateEnd));
		Dispatcher?.Dispatch(new GetListWithDates_Action(
			SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
	}

	void EditActionHandler(int id)
	{
		Logger!.LogDebug(string.Format("inside: {0}; id:{1}", nameof(MasterList) + "!" + nameof(EditActionHandler), id));
		Dispatcher?.Dispatch(new Get_Action(id, Enums.AddEditDisplay.Edit));
	}

	void DisplayActionHandler(int id)
	{
		Logger!.LogDebug(string.Format("inside: {0}; id:{1}", nameof(MasterList) + "!" + nameof(DisplayActionHandler), id));
		Dispatcher?.Dispatch(new Get_Action(id, Enums.AddEditDisplay.Display));
	}

	private async Task DeleteConfirmationHandler(int id, string title)
	{
		Logger!.LogDebug(string.Format("...{0}; id:{1}, title: {2}", nameof(MasterList) + "!" + nameof(DeleteConfirmationHandler), id, title));
		var parameters = new ModalParameters { { nameof(ConfirmDeleteModal.Message)
				, $"Special Event {title}" } };
		var modal = Modal.Show<ConfirmDeleteModal>("Confirmation Required", parameters);
		var result = await modal.Result;
		if (result.Confirmed)
		{
			Dispatcher?.Dispatch(new Delete_Action(id));
			Dispatcher?.Dispatch(new GetListWithDates_Action(
				SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
		}
	}

}