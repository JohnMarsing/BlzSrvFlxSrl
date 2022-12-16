using Ardalis.SmartEnum;

namespace BlzSrvFlxSrl.Features.SpecialEvents.Enums;

public abstract class VisibleComponet : SmartEnum<VisibleComponet>
{
	#region Id's
	private static class Id
	{
		internal const int MasterList = 1;  
		internal const int AddEditForm = 2;
		internal const int DisplayCard = 3;
	}
	#endregion

	#region  Declared Public Instances
	public static readonly VisibleComponet MasterList = new MasterListSE();
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

	private sealed class MasterListSE : VisibleComponet
	{
		public MasterListSE() : base($"{nameof(Id.MasterList)}", Id.MasterList) { }
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
