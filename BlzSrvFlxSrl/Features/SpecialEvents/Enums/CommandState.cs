using Ardalis.SmartEnum;

namespace BlzSrvFlxSrl.Features.SpecialEvents.Enums;

public abstract class CommandState : SmartEnum<CommandState>
{
	#region Id's
	private static class Id
	{
		internal const int Hidden = 1;
		internal const int Display = 2;
		internal const int Add = 3;
		internal const int Edit = 4;
		internal const int Delete = 5;
	}
	#endregion

	#region  Declared Public Instances
	public static readonly CommandState Hidden = new HiddenSE();
	public static readonly CommandState Display = new DisplaySE();
	public static readonly CommandState Add = new AddSE();
	public static readonly CommandState Edit = new EditSE();
	public static readonly CommandState Delete = new DeleteSE();
	#endregion


	private CommandState(string name, int value) : base(name, value)  // Constructor
	{
	}

	#region Extra Fields
	//public abstract string Title { get; }
	#endregion


	#region Private Instantiation

	private sealed class HiddenSE : CommandState
	{
		public HiddenSE() : base($"{nameof(Id.Hidden)}", Id.Hidden) { }
	}

	private sealed class DisplaySE : CommandState
	{
		public DisplaySE() : base($"{nameof(Id.Display)}", Id.Display) { }
	}

	private sealed class AddSE : CommandState
	{
		public AddSE() : base($"{nameof(Id.Add)}", Id.Add) { }
	}

	private sealed class EditSE : CommandState
	{
		public EditSE() : base($"{nameof(Id.Edit)}", Id.Edit) { }
	}

	private sealed class DeleteSE : CommandState
	{
		public DeleteSE() : base($"{nameof(Id.Delete)}", Id.Delete) { }
		//	public override string Title => "???";
	}

	#endregion
}
/*
public static class ActionButtons
	public const string AddIcon = "fas fa-plus";
	public const string AddButtonColor = "btn btn-success";
	public const string AddText = "Add";
	public const string AddModalText = "Save";

*/