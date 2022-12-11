﻿using BlzSrvFlxSrl.Features.SpecialEvents.Enums;
using Markdig;
using Microsoft.AspNetCore.Components;
using System;

namespace BlzSrvFlxSrl.Features.SpecialEvents.Models;

public class SpecialEvent
{
	public int Id { get; set; }
	public DateTime EventDate { get; set; }

	public int SpecialEventTypeId { get; set; }
	public string SpecialEventTypeName
	{
		get { return SpecialEventType.FromValue(SpecialEventTypeId).Name; 	}
	}

	public string? EventTypeDescr { get; set; }
	public int DaysDiff { get; set; }
	public string? DaysDiffDescr { get; set; }
	public string? Title { get; set; }
	public string? SubTitle { get; set; }
	public DateTime ShowBeginDate { get; set; }
	public DateTime ShowEndDate { get; set; }
	public string? ImageUrl { get; set; }
	public string? YouTubeId { get; set; }
	public string? WebsiteUrl { get; set; }
	public string? WebsiteDescr { get; set; }
	public string? Description { get; set; }  // ToDo: md?, probably going to be Component Body

	//public string DescriptionMD => Markdown.ToHtml(Description);

	public MarkupString DaysAheadMU
	{
		get
		{
			if (DaysDiffDescr != null)
			{
				if (DaysDiffDescr == "Days Ahead")
				{
					return (MarkupString)$"<span class='text-success'>{DaysDiff}</span> <i class='fas fa-angle-right'></i>";
				}
				else
				{
					return (MarkupString)$"<span class='text-danger'>{DaysDiff}</span> <i class='fas fa-angle-left'></i>"; // 
				}
			}
			else
			{
				return (MarkupString)"?";
			}

		}
	}


	public override string ToString()
	{
		return $"EventDate: {EventDate}, SpecialEventTypeId: {SpecialEventTypeId}";
	}

	public string EventDay()
	{
		return EventDate.Day.ToString();
	}

	public string EventYear()
	{
		return EventDate.Year.ToString();
	}

	public string EventMonth()
	{
		return EventDate.ToString("MMMM");
	}

	
}
