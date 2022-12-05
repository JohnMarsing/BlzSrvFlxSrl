namespace BlzSrvFlxSrl.Links;

public static class Account
{
	public const string Login = "/Account/Login";
	public const string Logout = "/Account/Logout";
	public const string LogoutAction = "Account/LogOut";
	public const string Profile = "/Account/Profile";

	public const string TitleAccessDenied = "Access Denied.";
	public const string TitleLogin = "Log in";
	public const string TitleLogout = "Log out";
	public const string TitleProfile = "Profile";

	public const string IconClaims = "fab fa-superpowers";
	public const string IconProfileVerified = "fas fa-check";
	public const string IconProfileNotVerified = "fas fa-question";
	public const string IconLogout = "fas fa-sign-out-alt";
	public const string IconLogin = "fas fa-sign-in-alt";
	public const string IconProfile = "fas fa-user";

	public static class PasswordChanged
	{
		public const string Index = "/account/PasswordChanged";
		public const string Title = " Password Changed Successfully";
		public const string PageTitle = " Password Changed";
		public const string Icon = "fas fa-key";
	}
}


public static class Home
{
	public const string Index = "/";
	public const string Title = "Blazor Server Fluxor Serilog";
	public const string ShortTitle = "Blz. Srv. Flx. Srl";
	public const string Acronym = "Blz. Srv. Flx. Srl"; 
	public const string PageTitle = "Home | BSFS";
	public const string Icon = "fas fa-home";  // fas fa-synagogue
	public const string Error = "/Error";
}

public static class SpecialEvents
{
	public const string Index = "/SpecialEvents/";
	public const string Title = "Special Events";
	public const string Icon = "far fa-clock";
}


public static class UpcomingEvents
{
	public const string Index = "/UpcomingEvents/";
	public const string Title = "Upcoming Events";
	public const string Icon = "far fa-clock";
}

public static class UpcomingEventsAdmin
{
	/*
	public static class Edit
	{
		public const string Page = "/UpcomingEventsAdminEdit";
		public const string Title = "Upcoming Events Edit";
		public const string Icon = "fas fa-pencil-alt";
	}
	*/
	public static class EditMarkdown
	{
		public const string Page = "/UpcomingEventsAdminEditMarkdown";
		public const string Title = "Edit Upcoming Events Markdown ";
		public const string Icon = "fab fa-markdown";
		public const string Icon2 = "fas fa-pencil-alt";
	}
	/*
	public static class Form
	{
		public const string Page = "/UpcomingEventsAdminForm";  // was UpcomingEventsCRUD
		public const string Title = "Upcoming Events Form";
		public const string Icon = "fas fa-tablet-alt";
	}

	public static class Grid
	{
		public const string Page = "/UpcomingEventsAdminGrid"; // SimpleGrid.razor
		public const string Title = "Upcoming Events Grid";
		public const string Icon = "far fa-clock";
	}

	public static class Table
	{
		public const string Page = "/UpcomingEventsAdminTable";
		public const string Title = "Upcoming Events Table";
		public const string Icon = "fas fa-table";
	}

	public static class UploadImage
	{
		public const string Page = "/UpcomingEventsAdminUploadImage";
		public const string Title = "Upload Upcoming Events Image ";
		public const string Icon = "fas fa-cloud-upload-alt";
		public const string Icon2 = "fas fa-image";
	}
	*/
}
