﻿namespace BlzSrvFlxSrl.Features.SpecialEvents;

public static class Constants
{
	public static PageHeaderVM GetPageHeaderForIndexVM()
	{
		return new PageHeaderVM { Title = "Index", Icon = "fas fa-list", Color = "text-primary", Id = 0 };
	}

	public static class SaveButton
	{
		public const string Icon = "fas fa-save";
		public const string Color = "btn btn-outline-success btn-sm";
	}

	public static class CancelButton
	{
		public const string Icon = "far fa-window-close"; 
		public const string Text = "Cancel";
		public const string Color = "btn btn-outline-secondary btn-sm";
	}


	public static class DateRange
	{
		public const string Start = "2021-03-01";
		public const string End = "2023-06-30";
	}


}

