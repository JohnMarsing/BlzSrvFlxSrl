using Microsoft.AspNetCore.Components;
using BlzSrvFlxSrl.Shared.Header.Enums;
using Blazored.Toast.Services;

namespace BlzSrvFlxSrl.Shared.Header;

public partial class BibleSearch
{
	[Inject] private IState<BibleSearchState>? BibleSearchState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }
	[Inject] public IToastService? Toast { get; set; }

	private async Task<IEnumerable<BibleBook>> SearchBibleBooks(string searchText)
	{
		return await Task.FromResult(BibleBook.List
			.Where(x => x.Title.ToLower().Contains(searchText.ToLower()))
			.OrderBy(o => o.Value));
	}
	/*
	private BibleBook? SelectedBook { get; set; }

	*/
	public string GetSelectedBook()
	{
		if (BibleSearchState!.Value.BibleBook is null)  // SelectedBook
		{
			return "BibleSearchState!.Value.BibleBook is null";
			//return "SelectedBook is null";
		}
		else
		{
			return $"BibleSearchState!.Value.BibleBook: {BibleSearchState!.Value.BibleBook.Name}";
			//return $"SelectedBook: {SelectedBook.Name}";  
		}
	}

	private void SelectedResultChanged(BibleBook bibleBook)
	{
		//Toast!.ShowInfo($"Dispatch Set Show List to {(bibleBook is null ? "BibleBook is null" : bibleBook.Abrv)}");

		//SelectedBook = bibleBook;
		Dispatcher!.Dispatch(new BibleSearchSetBibleBookAction(bibleBook!));  //SelectedBook!

		if (bibleBook is null)  //if (SelectedBook is null)
		{
			Dispatcher!.Dispatch(new BibleSearchSetShowBookChapterAnchorListAction(false));
		}
		else
		{
			Dispatcher!.Dispatch(new BibleSearchSetShowBookChapterAnchorListAction(true));
		}
	}

	/*
	public bool ListIsShowing = false;
	private string ShowListButtonColor => ListIsShowing ? "btn-success" : "btn-warning";
	private string ShowListButtonTitle => ListIsShowing ? "Hide Book Chapter List" : "Show Book Chapter List";
	private string ShowListButtonIcon => ListIsShowing ? "fas fa-chevron-up" : "fas fa-chevron-down";

	protected void ToggleButtonClick()
	{
		ListIsShowing = !ListIsShowing;
		Dispatcher!.Dispatch(new BibleSearchSetShowBookChapterAnchorListAction(ListIsShowing));
	}
	*/
}



