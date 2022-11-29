namespace BlzSrvFlxSrl.Shared.Header;

// 1. Action
public record BibleSearchSetBibleBookAction(Enums.BibleBook BibleBook);
public record BibleSearchSetWebsiteAction(Enums.BibleWebsite BibleWebsite);
public record BibleSearchSetShowBookChapterAnchorListAction(bool IsVisible);
public record BibleSearchSetShowWebsiteSelectAction(bool IsVisible);

// 2. State
public record BibleSearchState
{
	public Enums.BibleBook? BibleBook { get; init; }
	public Enums.BibleWebsite? BibleWebsite { get; init; }
	public bool ShowBookChapterAnchorList { get; init; }
	public bool ShowWebsiteSelect { get; init; }
}

// 3. Feature
public class HeaderStateFeature : Feature<BibleSearchState>
{
	public override string GetName() => "Header";

	protected override BibleSearchState GetInitialState()
	{
		return new BibleSearchState
		{
			// Don't set BibleBook because we don't want <BibleSearch> to be collapsed from it's details
			// BibleBook = Enums.BibleBook.Genesis,
			BibleWebsite = Enums.BibleWebsite.MyHebrewBible,
			ShowBookChapterAnchorList = false,
			ShowWebsiteSelect = false
		};
	}
}

// 4. Reducers
public static class BibleSearchReducers
{
	[ReducerMethod]
	public static BibleSearchState OnSetBibleBook(
		BibleSearchState state,
		BibleSearchSetBibleBookAction action)
	{
		return state with { BibleBook = action.BibleBook };
	}

	[ReducerMethod]
	public static BibleSearchState OnSetBibleWebsite(
		BibleSearchState state,
		BibleSearchSetWebsiteAction action)
	{
		return state with { BibleWebsite = action.BibleWebsite };
	}

	[ReducerMethod]
	public static BibleSearchState OnBibleSearchSetShowBookChapterAnchorList(
		BibleSearchState state, BibleSearchSetShowBookChapterAnchorListAction action)
	{
		return state with { ShowBookChapterAnchorList = action.IsVisible };
	}
	
	[ReducerMethod]
	public static BibleSearchState OnBibleSearchSetShowWebsiteSelect(
		BibleSearchState state, BibleSearchSetShowWebsiteSelectAction action)
	{
		return state with { ShowWebsiteSelect = action.IsVisible };
	}

}

// 5. Effects
