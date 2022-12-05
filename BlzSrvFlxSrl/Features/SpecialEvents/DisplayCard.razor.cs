using Blazored.Toast.Services;
using Fluxor;
using Markdig;
using Microsoft.AspNetCore.Components;
using System;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public partial class DisplayCard
{
	[Inject] private IState<SpecialEventsState>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }
	[Inject] public IToastService? Toast { get; set; }
	FormVM? FormVM => SpecialEventsState!.Value.Model;

	public bool ContentIsShowing = false;
	private string Color => ContentIsShowing ? "btn-success" : "btn-warning";
	private string Title => ContentIsShowing ? "Hide Video" : "Show Video";
	private string Icon => ContentIsShowing ? "fas fa-chevron-up" : "fas fa-chevron-down";

	/* 
	 if Description has no special md (e.g. Tables) then you don't need the pipeline
	 - https://stackoverflow.com/questions/67577034/blazor-webassembly-app-markdig-can-not-render-pipe-tables-or-grid-tables-in
	 */
	private string GetDescriptionMdPipeline()
	{
		MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
		if (FormVM!.Description is null)
		{
			return "null";
		}
		else
		{
			return Markdig.Markdown.ToHtml(FormVM!.Description, pipeline);
		}

	}


	protected void ToggleButtonClick()
	{
		ContentIsShowing = !ContentIsShowing;
		Toast!.ShowInfo($"{Title} clicked");
		//ShowVideo = !ShowVideo;
		//SetShowVideoButton();
		//StateHasChanged();
	}

	private void Edit_ButtonClick(int id)
	{
		Toast!.ShowInfo($"{nameof(Edit_ButtonClick)} clicked");
		//NavManager.NavigateTo(Links.UpcomingEventsAdmin.EditMarkdown.Page + "/" + id);
	}
}



