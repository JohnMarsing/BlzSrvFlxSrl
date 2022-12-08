using Microsoft.AspNetCore.Components;
using Blazored.Modal.Services;
using BlzSrvFlxSrl.Shared.Modals;

namespace BlzSrvFlxSrl.Features.ModalPages;

public partial class Airplane
{
	[Inject] public ILogger<Airplane>? Logger { get; set; }
	[CascadingParameter] IModalService Modal { get; set; } = default!;

	private async Task ShowModal()
	{
		var sr71Modal = Modal.Show<Sr71>("SR 71");
		var result = await sr71Modal.Result;
		if (result.Cancelled)
		{
			Logger!.LogDebug(string.Format("Inside {0}; Modal was CANCELED"
					, nameof(Airplane) + "!" + nameof(ShowModal)));
		}
		else if (result.Confirmed)
		{
			Logger!.LogDebug(string.Format("Inside {0}; Modal was CLOSED"
					, nameof(Airplane) + "!" + nameof(ShowModal)));
		}
	}
}