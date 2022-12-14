using Microsoft.AspNetCore.Components;
using Blazored.Toast.Services;

namespace BlzSrvFlxSrl.Shared.Header;

public partial class ToasterBibleSearch
{
	[Inject] public IToastService? Toast { get; set; }

	protected override void OnInitialized()
	{
		SubscribeToAction<ShowDetails_Action>(BibleDetails_ShowIsVisible_Toast);
		base.OnInitialized();
	}

	private void BibleDetails_ShowIsVisible_Toast(ShowDetails_Action action)
	{
		Toast!.ShowInfo($"BibleDetails!ShowDetails action; IsVisible: {action.IsVisible}");
	}

}


