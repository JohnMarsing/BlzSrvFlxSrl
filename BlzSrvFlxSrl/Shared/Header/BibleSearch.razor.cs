using Microsoft.AspNetCore.Components;
using BlzSrvFlxSrl.Shared.Header.Enums;

namespace BlzSrvFlxSrl.Shared.Header;

public partial class BibleSearch
{
	[Inject] private IState<BibleSearchState>? BibleSearchState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	private async Task<IEnumerable<BibleBook>> SearchBibleBooks(string searchText)
	{
		return await Task.FromResult(BibleBook.List
			.Where(x => x.Title.ToLower().Contains(searchText.ToLower()))
			.OrderBy(o => o.Value));
	}

	private void SelectedResultChanged(BibleBook bibleBook)
	{
		Dispatcher!.Dispatch(new BibleSearchSetBibleBookAction(bibleBook!)); 

		if (bibleBook is null)  
		{
			Dispatcher!.Dispatch(new BibleSearchSetShowBookChapterAnchorListAction(false));
			Dispatcher!.Dispatch(new BibleSearchSetShowWebsiteSelectAction(false));
		}
		else
		{
			Dispatcher!.Dispatch(new BibleSearchSetShowBookChapterAnchorListAction(true));
			Dispatcher!.Dispatch(new BibleSearchSetShowWebsiteSelectAction(true));
		}
	}

}



