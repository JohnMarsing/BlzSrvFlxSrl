﻿using BlzSrvFlxSrl.Features.SpecialEvents.Enums;
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

	public string EventTypeDescr { get; set; }
	public int DaysDiff { get; set; }
	public string DaysDiffDescr { get; set; }
	public string Title { get; set; }
	public string SubTitle { get; set; }
	public DateTime ShowBeginDate { get; set; }
	public DateTime ShowEndDate { get; set; }
	public string ImageUrl { get; set; }
	public string YouTubeId { get; set; }
	public string WebsiteUrl { get; set; }
	public string WebsiteDescr { get; set; }
	public string Description { get; set; }  // ToDo: md?, probably going to be Component Body

	public string DaysAhead
	{
		get
		{
			if (DaysDiffDescr != null)
			{
				if (DaysDiff >= 0)
				{
					return DaysDiff + ' ' + DaysDiffDescr;
				}
				else
				{
					return DaysDiff + ' ' + DaysDiffDescr;
				}
			}
			else
			{
				return "?";
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