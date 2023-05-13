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

	protected override void OnInitialized()
	{
		Logger!.LogDebug(string.Format("Inside {0}", nameof(MasterList) + "!" + nameof(OnInitialized)));
		if (SpecialEventsState!.Value.SpecialEventList is null)
		{
			Dispatcher!.Dispatch(new Get_List_Action(
				SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
		}
		base.OnInitialized();
	}


	private async Task ReturnedCrud(CrudAndIdArgs args) // CallBackEventArgs: Enum.Crud, int Id
	{
		int id = args.Id;

		string crudName;
		if (args.Crud == null)
		{
			Logger!.LogWarning(string.Format("inside: {0}; args.Crud: == null", nameof(MasterList) + "!" + nameof(ReturnedCrud)));
			crudName = "Display";
		}
		else
		{
			crudName = args.Crud.Name;
		}

		Logger!.LogDebug(string.Format("inside: {0}; crudName: {1}", nameof(MasterList), crudName));  //; id:{1} ... , id));
		switch (crudName)
		{
			case "Add":
				Dispatcher!.Dispatch(new Add_Action());
				break;

			case "Edit":
				Dispatcher!.Dispatch(new Get_Item_Action(id, Enums.AddEditDisplay.Edit));
				break;

			case "Display":
				Dispatcher!.Dispatch(new Get_Item_Action(id, Enums.AddEditDisplay.Display));
				break;

			case "Delete":
				if (await IsModalConfirmed(id) == true)
				{
					Dispatcher!.Dispatch(new Delete_Action(id));
					Dispatcher!.Dispatch(new Get_List_Action(
						SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
				}

				break;

			case "Repopulate":
				Dispatcher!.Dispatch(new Get_List_Action(SpecialEventsState!.Value.DateBegin, SpecialEventsState.Value.DateEnd));
				break;

			default:
				break;
		}
	}


	private async Task<bool> IsModalConfirmed(int id)
	{
		var parameters = new ModalParameters { { nameof(ConfirmDeleteModal.Message), $"Special Event Id: {id}" } };
		var modal = Modal.Show<ConfirmDeleteModal>("Confirmation Required", parameters);
		var result = await modal.Result;
		return result.Confirmed;

	}


}