using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class ToasterSpecialEvents
{
	[Inject] public IToastService? Toast { get; set; }
	protected override void OnInitialized()
	{
		SubscribeToAction<GetListSuccess_Action>(GetListSuccess_Toast);
		SubscribeToAction<GetListWarning_Action>(GetListWarning_Toast);
		SubscribeToAction<GetListFailure_Action>(GetListFailure_Toast);

		SubscribeToAction<SetDateRange_Action>(SetDateRange_Toast);
		SubscribeToAction<GetSuccess_Action>(GetSuccess_Toast);
		SubscribeToAction<GetWarning_Action>(GetWarning_Toast);
		SubscribeToAction<GetFailure_Action>(GetFailure_Toast);

		SubscribeToAction<SubmitSuccess_Action>(SubmitSuccess_Toast);
		SubscribeToAction<SubmitFailure_Action>(SubmitFailure_Toast);

		SubscribeToAction<DeleteSuccess_Action>(DeleteSuccess_Toast);
		SubscribeToAction<DeleteFailure_Action>(DeleteFailure_Toast);

		base.OnInitialized();
	}

	private void GetListSuccess_Toast(GetListSuccess_Action action)
	{
		Toast!.ShowInfo($"Got list of {action.SpecialEvents.Count} records");
	}
	private void GetListWarning_Toast(GetListWarning_Action action)
	{
		Toast!.ShowWarning($"No records found");
	}
	private void GetListFailure_Toast(GetListFailure_Action action)
	{
		Toast!.ShowError($"{action.ErrorMessage}");
	}
	private void SetDateRange_Toast(SetDateRange_Action action)
	{
		Toast!.ShowInfo($"Selected Date Range: {action.DateBegin.ToString("yyyy-MM-dd")} to {action.DateEnd.ToString("yyyy-MM-dd")}");
	}
	private void GetSuccess_Toast(GetSuccess_Action action)
	{
		Toast!.ShowInfo($"Got {action.Model!.Title!}");
	}
	private void GetWarning_Toast(GetWarning_Action action)
	{
		if (action.Warning)
		{
			Toast!.ShowWarning($"{action.WarningMessage}");
		}
		else
		{
			Toast!.ShowError($"{action.WarningMessage}");
		}
	}
	private void GetFailure_Toast(GetFailure_Action action)
	{
		if (action.Warning)
		{
			Toast!.ShowWarning($"{action.ErrorMessage}");
		}
		else
		{
			Toast!.ShowError($"{action.ErrorMessage}");
		}
	}

	private void SubmitSuccess_Toast(SubmitSuccess_Action action)
	{
		Toast!.ShowSuccess($"{action.SuccessMessage}");
	}
	private void SubmitFailure_Toast(SubmitFailure_Action action)
	{
		Toast!.ShowError($"Form submit error; ErrorMessage: {action.ErrorMessage}");
	}

	private void DeleteSuccess_Toast(DeleteSuccess_Action action)
	{
		Toast!.ShowSuccess(action.SuccessMessage);
	}

	private void DeleteFailure_Toast(DeleteFailure_Action action)
	{
		Toast!.ShowError($"Failed to delete Special Events; {action.ErrorMessage}");
	}


}
