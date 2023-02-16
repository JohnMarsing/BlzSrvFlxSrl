using Ardalis.SmartEnum;

namespace BlzSrvFlxSrl.Features.SpecialEvents.Enums;

public abstract class AddEditDisplay : SmartEnum<AddEditDisplay>
{
	#region Id's
	private static class Id
	{
		internal const int Add = 1;
		internal const int Edit = 2;
		internal const int Display = 3;
	}
	#endregion

	#region  Declared Public Instances
	public static readonly AddEditDisplay Add = new AddSE();
	public static readonly AddEditDisplay Edit = new EditSE();
	public static readonly AddEditDisplay Display = new DisplaySE();
	#endregion

	private AddEditDisplay(string name, int value) : base(name, value)  // Constructor
	{
	}

	#region Extra Fields
	public abstract string FormTitle { get; }
	public abstract string FormSubmitButton { get; }
	public abstract VisibleComponet VisibleComponet { get; }
	#endregion


	#region Private Instantiation

	private sealed class AddSE : AddEditDisplay
	{
		public AddSE() : base($"{nameof(Id.Add)}", Id.Add) { }
		public override string FormTitle => "Add";
		public override string FormSubmitButton => "Add Event";
		public override VisibleComponet VisibleComponet => VisibleComponet.AddEditForm;
	}

	private sealed class EditSE : AddEditDisplay
	{
		public EditSE() : base($"{nameof(Id.Edit)}", Id.Edit) { }
		public override string FormTitle => "Edit";
		public override string FormSubmitButton => "Update Event";
		public override VisibleComponet VisibleComponet => VisibleComponet.AddEditForm;
	}

	private sealed class DisplaySE : AddEditDisplay
	{
		public DisplaySE() : base($"{nameof(Id.Display)}", Id.Display) { }
		public override string FormTitle => "N/A";
		public override string FormSubmitButton => "N/A";
		public override VisibleComponet VisibleComponet => VisibleComponet.DisplayCard;
	}

	#endregion
}
