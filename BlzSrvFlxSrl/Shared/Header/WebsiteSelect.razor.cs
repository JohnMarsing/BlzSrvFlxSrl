using Microsoft.AspNetCore.Components;
using BlzSrvFlxSrl.Shared.Header.Enums;


namespace BlzSrvFlxSrl.Shared.Header;

public partial class WebsiteSelect
{
	[Inject] private IState<BibleSearchState>? BibleSearchState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }

	private string? selectedBibleWebsite = BibleWebsite.MyHebrewBible.Name;

	private bool IsSelectedBibleWebsite(string bibleWebsite)
	{
		return bibleWebsite == selectedBibleWebsite;
	}

	private void ChangingBibleWebsite(ChangeEventArgs e)
	{
		selectedBibleWebsite = e.Value?.ToString() ?? ""; // selectedBibleWebsite = e.Value.ToString();
		Dispatcher!.Dispatch(new BibleSearchSetBibleWebsiteAction(BibleWebsite.FromName(selectedBibleWebsite)));
	}

}
