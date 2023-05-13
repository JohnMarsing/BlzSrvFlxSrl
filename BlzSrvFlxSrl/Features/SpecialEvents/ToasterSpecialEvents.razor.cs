using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class ToasterSpecialEvents
{
	[Inject] public IToastService? Toast { get; set; }
	protected override void OnInitialized()
	{
		//SubscribeToAction<Get_List_Success_Action>(Get_List_Success_Toast); // Too much chatter
		SubscribeToAction<Get_List_Warning_Action>(Get_List_Warning_Toast);
		SubscribeToAction<Get_List_Failure_Action>(Get_List_Failure_Toast);

		SubscribeToAction<Get_Item_Success_Action>(Get_Item_Success_Toast);
		SubscribeToAction<Get_Item_Warning_Action>(Get_Item_Warning_Toast);
		SubscribeToAction<Get_Item_Failure_Action>(Get_Item_Failure_Toast);

		SubscribeToAction<Submited_Response_Success_Action>(Submited_Response_Success_Toast);
		SubscribeToAction<Submited_Response_Failure_Action>(Submited_Response_Failure_Toast);

		SubscribeToAction<DeleteSuccess_Action>(DeleteSuccess_Toast);
		SubscribeToAction<DeleteFailure_Action>(DeleteFailure_Toast);

		//SubscribeToAction<SetDateRange_Action>(SetDateRange_Toast);
		base.OnInitialized();
	}

	//private void Get_List_Success_Toast(Get_List_Success_Action action) => Toast!.ShowInfo($"Got list of {action.SpecialEvents.Count} records");

	private void Get_List_Warning_Toast(Get_List_Warning_Action action) => Toast!.ShowWarning($"No records found");
	private void Get_List_Failure_Toast(Get_List_Failure_Action action) => Toast!.ShowError($"{action.ErrorMessage}");

	private void Get_Item_Success_Toast(Get_Item_Success_Action action) => Toast!.ShowInfo($"Got {action.Model!.Title!}");
	private void Get_Item_Warning_Toast(Get_Item_Warning_Action action) => Toast!.ShowWarning($"{action.WarningMessage}");
	private void Get_Item_Failure_Toast(Get_Item_Failure_Action action) => Toast!.ShowError($"{action.ErrorMessage}");

	private void Submited_Response_Success_Toast(Submited_Response_Success_Action action) => Toast!.ShowSuccess($"{action.SuccessMessage}");
	private void Submited_Response_Failure_Toast(Submited_Response_Failure_Action action) => Toast!.ShowError($"Form submit error; ErrorMessage: {action.ErrorMessage}");
	private void DeleteSuccess_Toast(DeleteSuccess_Action action) => Toast!.ShowSuccess(action.SuccessMessage);
	private void DeleteFailure_Toast(DeleteFailure_Action action) => Toast!.ShowError($"Failed to delete Special Events; {action.ErrorMessage}");

	//private void SetDateRange_Toast(SetDateRange_Action action) => Toast!.ShowInfo($"Selected Date Range: {action.DateBegin.ToString("yyyy-MM-dd")} to {action.DateEnd.ToString("yyyy-MM-dd")}");


}
