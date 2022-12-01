
using Microsoft.AspNetCore.Components;
using Blazored.Toast.Services;
using BlzSrvFlxSrl.Shared.Header;
using BlzSrvFlxSrl.Features.SpecialEvents;
//using System.Net.NetworkInformation;

namespace BlzSrvFlxSrl.Shared;

public partial class Toaster
{
	[Inject] public IToastService Toast { get; set; }

	protected override void OnInitialized()
	{
		SubscribeToAction<BibleSearch_ShowDetails_Action>(BibleDetails_ShowIsVisible_Toast);
		SubscribeToAction<SpecialEvents_SetDateRange_Action>(SpecialEvents_SetDateRange_Toast);
		base.OnInitialized();
	}    

	private void BibleDetails_ShowIsVisible_Toast(BibleSearch_ShowDetails_Action action)
	{
		Toast.ShowInfo($"BibleDetails!ShowDetails action; IsVisible: {action.IsVisible}");
	}

	private void SpecialEvents_SetDateRange_Toast(SpecialEvents_SetDateRange_Action action)
	{
		Toast.ShowInfo($"SpecialEvents!SetDateRange action; Date Range: {action.DateBegin.ToString("yyyy-MM-dd")} to {action.DateEnd.ToString("yyyy-MM-dd")}");
	}

}

