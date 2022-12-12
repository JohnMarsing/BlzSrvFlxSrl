﻿namespace BlzSrvFlxSrl.Features.SpecialEvents;

public static class Command
{
	public const string Read = "Read";
	public const string Add = "Add";
	public const string Edit = "Edit";
	public const string Delete = "Delete";
}

public static class ActionButtons
{
	public const string Index = "/Admin/WeeklyVideos";
	public const string Title = "Weekly Videos";
	public const string Icon = "fab fa-youtube";

	public const string AddIcon = "fas fa-plus";
	public const string AddButtonColor = "btn btn-success";
	public const string AddText = "Add";
	public const string AddModalText = "Save";

	public const string EditIcon = "fas fa-pencil-alt";
	public const string EditButtonColor = "btn btn-primary";
	public const string EditText = "Edit";
	public const string EditModalText = "Update";

	public const string DeleteIcon = "fa fa-times";
	public const string DeleteButtonColor = "btn btn-danger";
	public const string DeleteText = "Delete";

	public const string DisplayIcon = "fa fa-binoculars";
	public const string DisplayButtonColor = "btn btn-info";
	public const string DisplayText = "Display";

	public const string PopulateIcon = "fas fa-retweet"; 
	public const string PopulateButtonColor = "btn btn-warning";
	public const string PopulateText = "Re-Populate";

	public const string SaveIcon = "fas fa-save";
	public const string SaveButtonColor = "btn btn-success btn-sm";

	public const string CancelIcon = "far fa-window-close"; //"far fa-window-close";
	public const string CancelText = "Cancel";
	public const string CancelButtonColor = "btn btn-secondary btn-sm";
}
