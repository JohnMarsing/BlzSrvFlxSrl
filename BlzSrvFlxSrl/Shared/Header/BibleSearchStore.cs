
using System;

namespace BlzSrvFlxSrl.Shared.Header;

// 1. Action
public record BibleSearchSetBibleBookAction(Enums.BibleBook BibleBook);
public record BibleSearchSetBibleWebsiteAction(Enums.BibleWebsite BibleWebsite);
public record BibleSearchSetShowBookChapterAnchorListAction(bool IsVisible);
public record BibleSearchSetShowWebsiteSelectAction();

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
			//BibleBook = Enums.BibleBook.Genesis,
			BibleWebsite = Enums.BibleWebsite.MyHebrewBible,
			ShowBookChapterAnchorList = false,
			ShowWebsiteSelect = false
		};
	}
}

// 4. Reducers
public static class BibleSearchReducers
{


	// Enums.BibleBook bibleBook
	[ReducerMethod]
	public static BibleSearchState OnSetBibleBook(
		BibleSearchState state,
		BibleSearchSetBibleBookAction action)
	{
		return state with { BibleBook = action.BibleBook };
	}

	[ReducerMethod]
	public static BibleSearchState OSetBibleWebsite(BibleSearchState state, Enums.BibleWebsite bibleWebsite)
	{
		return state with { BibleWebsite = bibleWebsite };
	}

	[ReducerMethod]
	public static BibleSearchState OnBibleSearchSetShowBookChapterAnchorList(
		BibleSearchState state, BibleSearchSetShowBookChapterAnchorListAction action)
	{
		return state with { ShowBookChapterAnchorList = action.IsVisible };
	}

	[ReducerMethod(typeof(BibleSearchSetShowWebsiteSelectAction))]
	public static BibleSearchState OnBibleSearchSetShowWebsiteSelect(BibleSearchState state)
	{
		return state with { ShowWebsiteSelect = false };
	}

	/*
		[ReducerMethod(typeof(BibleSearchSetBibleBookAction))]
		public static BibleSearchState OnSetBibleBook(BibleSearchState state, Enums.BibleBook bibleBook)
		{
			return state with { BibleBook = bibleBook };
		}

		[ReducerMethod(typeof(BibleSearchSetBibleWebsiteAction))]
		public static BibleSearchState OSetBibleWebsite(BibleSearchState state, Enums.BibleWebsite bibleWebsite)
		{
			return state with { BibleWebsite = bibleWebsite };
		}
	*/
}

// 5. Effects
