using Microsoft.AspNetCore.Components;
using BlzSrvFlxSrl.Shared.Header.Enums;

namespace BlzSrvFlxSrl.Shared.Header;

public partial class BookChapterAnchorList
{
	[Inject] private IState<State>? BibleSearchState { get; set; }

	protected string? AnchorBookChapterUrl(int chapter)
	{
		if (BibleSearchState!.Value.BibleWebsite == BibleWebsite.MyHebrewBible)
		{
			return $"{BibleWebsite.MyHebrewBible.UrlBase}{BibleSearchState!.Value!.BibleBook!.Title}/{chapter}{UrlSuffix(true)}";
		}
		else
		{
			// https://www.stepbible.org/?q=version=LBLA|reference=Gen.1&options=NVUGVH&display=INTERLEAVED
			//https://www.stepbible.org/?q=version=LBLA|reference=Matt.3&options=HNVUG
			if (BibleSearchState.Value.BibleWebsite == BibleWebsite.StepBibleSpanish)
			{
				if (BibleSearchState!.Value!.BibleBook!.Value < 40)
				{ 
					return $"{BibleWebsite.StepBibleSpanish.UrlBase}?q=version=LBLA|reference={BibleSearchState.Value.BibleBook.Abrv}.{chapter}&options=NVUGVH&display=INTERLEAVED"; 
				}
				else
				{
					return $"{BibleWebsite.StepBibleSpanish.UrlBase}?q=version=LBLA|reference={BibleSearchState.Value.BibleBook.Abrv}.{chapter}&options=HNVUG";
				}
					
			}
			else
			{
				if (BibleSearchState!.Value!.BibleBook!.Value < 40)
				{
					return $"{BibleWebsite.StepBible.UrlBase}{Versions(true)}|reference={BibleSearchState.Value.BibleBook.Abrv}.{chapter}{UrlSuffix(true)}";
				}
				else
				{
					return $"{BibleWebsite.StepBible.UrlBase}{Versions(false)}|reference={BibleSearchState.Value.BibleBook.Abrv}.{chapter}{UrlSuffix(false)}";
				}
			}

		}
	}

	protected string? BookChapterTitle(int chapter)
	{
		if (BibleSearchState!.Value.BibleWebsite == BibleWebsite.MyHebrewBible)
		{
			return $"{BibleWebsite.MyHebrewBible.UrlTitle}{BibleSearchState!.Value!.BibleBook!.Title}/{chapter}{UrlSuffix(true)}";
		}
		else
		{
			if (BibleSearchState!.Value!.BibleBook!.Value < 40)
			{
				return $"{BibleWebsite.StepBible.UrlTitle} {BibleSearchState.Value.BibleBook.Abrv}.{chapter} OT";
			}
			else
			{
				return $"{BibleWebsite.StepBible.UrlTitle} {BibleSearchState.Value.BibleBook.Abrv}.{chapter} NT";
			}
			
		}
	}

	private string Versions(bool isOT) 
	{
		if (BibleSearchState!.Value.BibleWebsite == BibleWebsite.MyHebrewBible)
		{
			return "";
		}
		else
		{
			if (isOT)
			{
				return "?q=version=KJVA|version=THOT|version=LXX";
			}
			else
			{
				return "?q=version=KJVA|version=THGNT";
			}
		}
	}

	private string UrlSuffix(bool isOT)
	{
		if (BibleSearchState!.Value.BibleWebsite == BibleWebsite.MyHebrewBible)
		{
			return "/slug";
		}
		else
		{
			if (isOT)
			{
				return "&options=UVNGVH&display=INTERLEAVED";
			}
			else
			{
				return "&options=GVUNH";
			}
		}
	}

}
