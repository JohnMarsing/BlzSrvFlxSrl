using Blazored.Modal.Services;
using BlzSrvFlxSrl.Shared.Modals;
using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.ModalPages;

public partial class Delete
{
	//[Inject] public ILogger<Delete>? Logger { get; set; }
	[CascadingParameter] IModalService Modal { get; set; } = default!;

	private async Task ShowModal()
	{
		var modal = Modal.Show<ConfirmDelete>(ShowModalButton.TitleForModal);
		var result = await modal.Result;

		/*	*/
		if (result.Confirmed)
		{
			//Logger!.LogDebug(string.Format("Inside {0}; Modal was CLOSED", nameof(Delete) + "!" + nameof(ShowModal)));
		}
		
		/*
		if (result.Cancelled)
		{
			Logger!.LogDebug(string.Format("Inside {0}; Modal was CANCELED", nameof(Delete) + "!" + nameof(ShowModal)));
		}
		*/
	
	}
}

public static class ShowModalButton
{
	public const string Color = "btn-warning";
	public const string Title = "Show Delete Modal";
	public const string TitleForModal = "Confirmation Required";
}