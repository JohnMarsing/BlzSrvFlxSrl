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
		Dispatcher!.Dispatch(new BibleSearch_SetBibleBook_Action(bibleBook!)); 

		if (bibleBook is null)  
		{
			Dispatcher!.Dispatch(new BibleSearch_ShowDetails_Action(false));
		}
		else
		{
			Dispatcher!.Dispatch(new BibleSearch_ShowDetails_Action(true));
		}
	}

}



