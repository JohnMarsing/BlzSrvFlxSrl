namespace BlzSrvFlxSrl.Shared.Header;

// 1. Action
public record BibleSearch_SetBibleBook_Action(Enums.BibleBook BibleBook);
public record BibleSearch_SetWebsite_Action(Enums.BibleWebsite BibleWebsite);
public record ShowDetails_Action(bool IsVisible);

// 2. State
public record BibleSearchState
{
	public Enums.BibleBook? BibleBook { get; init; }
	public Enums.BibleWebsite? BibleWebsite { get; init; }
	public bool ShowDetails { get; init; }
}

// 3. Feature
public class HeaderStateFeature : Feature<BibleSearchState>
{
	public override string GetName() => "Header";

	protected override BibleSearchState GetInitialState()
	{
		return new BibleSearchState
		{
			// Don't set default BibleBook (e.g. Genesis) because we don't want <BibleSearch> to be collapsed from it's details
			BibleWebsite = Enums.BibleWebsite.MyHebrewBible,
			ShowDetails = false
		};
	}
}

// 4. Reducers
public static class BibleSearchReducers
{
	[ReducerMethod]
	public static BibleSearchState OnSetBibleBook(
		BibleSearchState state,
		BibleSearch_SetBibleBook_Action action)
	{
		return state with { BibleBook = action.BibleBook };
	}

	[ReducerMethod]
	public static BibleSearchState OnSetBibleWebsite(
		BibleSearchState state,
		BibleSearch_SetWebsite_Action action)
	{
		return state with { BibleWebsite = action.BibleWebsite };
	}

	[ReducerMethod]
	public static BibleSearchState OnBibleSearchShowDetailsAction(
		BibleSearchState state, ShowDetails_Action action)
	{
		return state with { ShowDetails = action.IsVisible };
	}


}

// 5. Effects
