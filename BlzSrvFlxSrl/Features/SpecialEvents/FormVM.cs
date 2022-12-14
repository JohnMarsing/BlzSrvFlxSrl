using BlzSrvFlxSrl.Features.SpecialEvents.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlzSrvFlxSrl.Features.SpecialEvents;

public class FormVM
{
	[Required]
	[Key]
	public int Id { get; set; }

	[Required(ErrorMessage = "The Start Date field is required.")]
	public DateTime? ShowBeginDate { get; set; }

	[Required(ErrorMessage = "The End Date field is required.")]
	public DateTime? ShowEndDate { get; set; }

	[Required]
	public DateTime EventDate { get; set; }

	[Required(ErrorMessage = "An event type is required")]
	public int SpecialEventTypeId { get; set; }

	[Required]
	public string? Title { get; set; }  // NOT NULL

	public string? SubTitle { get; set; }

	[DataType(DataType.ImageUrl)]
	public string? ImageUrl { get; set; }

	public string? YouTubeId { get; set; }

	[DataType(DataType.Url)]
	public string? WebsiteUrl { get; set; }

	public string? WebsiteDescr { get; set; }
	public string? Description { get; set; }  // ToDo: md?, probably going to be Component Body

	// ToDo: Warning	CS8618	Non-nullable property 'Description' et. al. must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
	public FormVM()
	{
		SpecialEventTypeId = SpecialEventType.Other.Value;
		EventDate = DateTime.Now.AddDays(35);
		ShowBeginDate = DateTime.Now.AddMonths(1);
		ShowEndDate = DateTime.Now.AddDays(40);
	}

}


/*
[Required(ErrorMessage = "The Start Date field is required.")]
[Range(typeof(DateTime), "3/5/2021", "3/25/2021", ParseLimitsInInvariantCulture = true,
	ErrorMessage = "The start date should not be beyond 5 March 2021.")]
public DateTime? StartDate { get; set; } = null;

[Required(ErrorMessage = "The End Date field is required.")]
[Range(typeof(DateTime), "3/5/2021", "3/25/2021", ParseLimitsInInvariantCulture = true,
	ErrorMessage = "The end date should not be above 25 March 2021.")]
[HandleCustomValidation]
public DateTime? EndDate { get; set; } = null;		 
 */
