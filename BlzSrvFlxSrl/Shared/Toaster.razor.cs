
using Microsoft.AspNetCore.Components;
using Blazored.Toast.Services;
using BlzSrvFlxSrl.Shared.Header;
using BlzSrvFlxSrl.Features.SpecialEvents;
//using System.Net.NetworkInformation;

namespace BlzSrvFlxSrl.Shared;

public partial class Toaster
{
	[Inject] public IToastService? Toast { get; set; }

	protected override void OnInitialized()
	{
		SubscribeToAction<BibleSearch_ShowDetails_Action>(BibleDetails_ShowIsVisible_Toast);
		SubscribeToAction<SpecialEvents_SetDateRange_Action>(SpecialEvents_SetDateRange_Toast);
		SubscribeToAction<SpecialEvents_GetFailure_Action>(SpecialEvents_GetFailure_Toast);
		SubscribeToAction<SpecialEvents_GetSuccess_Action>(SpecialEvents_GetSuccess_Toast);

		SubscribeToAction<SpecialEvents_SubmitSuccess_Action>(SpecialEvents_SubmitSuccess_Toast);
		SubscribeToAction<SpecialEvents_SubmitFailure_Action>(SpecialEvents_SubmitFailure_Toast);

		SubscribeToAction<SpecialEvents_DeleteSuccess_Action>(SpecialEvents_DeleteSuccess_Toast);
		SubscribeToAction<SpecialEvents_DeleteFailure_Action>(SpecialEvents_DeleteFailure_Toast);

		base.OnInitialized();
	}    


	private void BibleDetails_ShowIsVisible_Toast(BibleSearch_ShowDetails_Action action)
	{
		Toast!.ShowInfo($"BibleDetails!ShowDetails action; IsVisible: {action.IsVisible}");
	}

	private void SpecialEvents_SetDateRange_Toast(SpecialEvents_SetDateRange_Action action)
	{
		Toast!.ShowInfo($"SpecialEvents!SetDateRange action; Date Range: {action.DateBegin.ToString("yyyy-MM-dd")} to {action.DateEnd.ToString("yyyy-MM-dd")}");
	}

	private void SpecialEvents_GetSuccess_Toast(SpecialEvents_GetSuccess_Action action)
	{
		Toast!.ShowInfo($"Got {action.Model!.Title!}");
	}

	private void SpecialEvents_GetFailure_Toast(SpecialEvents_GetFailure_Action action)
	{
		Toast!.ShowError($"{action.ErrorMessage}");
	}

	private void SpecialEvents_SubmitSuccess_Toast(SpecialEvents_SubmitSuccess_Action action)
	{
		Toast!.ShowSuccess($"{action.SuccessMessage}");
	}
	private void SpecialEvents_SubmitFailure_Toast(SpecialEvents_SubmitFailure_Action action)
	{
		Toast!.ShowError($"SpecialEvents!SubmitFailure action; ErrorMessage: {action.ErrorMessage}");
	}


	private void SpecialEvents_DeleteSuccess_Toast(SpecialEvents_DeleteSuccess_Action action)
	{
		Toast!.ShowSuccess($"Special Event Deleted");
	}
	private void SpecialEvents_DeleteFailure_Toast(SpecialEvents_DeleteFailure_Action action)
	{
		Toast!.ShowError($"SpecialEvents!DeleteFailure action; ErrorMessage: {action.ErrorMessage}");
	}


}

