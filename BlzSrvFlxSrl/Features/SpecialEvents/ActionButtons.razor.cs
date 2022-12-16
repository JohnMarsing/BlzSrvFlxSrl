using BlzSrvFlxSrl.Features.SpecialEvents.Enums;
using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class ActionButtons
{
	[Parameter] public Crud? ParmCrud { get; set; } 
	[Parameter] public int Id { get; set; }
	[Parameter]	public EventCallback<CallBackEventArgs> OnCallBackEvent { get; set; }

	private async Task OnButtonClicked()
	{
		CallBackEventArgs args = new CallBackEventArgs
		{
			Crud=ParmCrud,
			Id=Id
		};
		await OnCallBackEvent.InvokeAsync(args);
	}
}

public struct CallBackEventArgs
{
	public Crud Crud { get; set; }
	public int Id { get; set; }
}

