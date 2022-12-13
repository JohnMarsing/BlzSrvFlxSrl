using Ardalis.SmartEnum;

namespace BlzSrvFlxSrl.Features.SpecialEvents.Enums;

public abstract class VisibleComponet : SmartEnum<VisibleComponet>
{
	#region Id's
	private static class Id
	{
		internal const int Table = 1;  
		internal const int AddEditForm = 2;
		internal const int DisplayCard = 3;
	}
	#endregion

	#region  Declared Public Instances
	public static readonly VisibleComponet Table = new TableSE();
	public static readonly VisibleComponet AddEditForm = new AddEditFormSE();
	public static readonly VisibleComponet DisplayCard = new DisplayCardSE();
	#endregion

	private VisibleComponet(string name, int value) : base(name, value)  // Constructor
	{
	}

	#region Extra Fields
	//public abstract string Title { get; }
	#endregion


	#region Private Instantiation

	private sealed class TableSE : VisibleComponet
	{
		public TableSE() : base($"{nameof(Id.Table)}", Id.Table) { }
	}

	private sealed class AddEditFormSE : VisibleComponet
	{
		public AddEditFormSE() : base($"{nameof(Id.AddEditForm)}", Id.AddEditForm) { }
	}

	private sealed class DisplayCardSE : VisibleComponet
	{
		public DisplayCardSE() : base($"{nameof(Id.DisplayCard)}", Id.DisplayCard) { }
	}

	#endregion
}
