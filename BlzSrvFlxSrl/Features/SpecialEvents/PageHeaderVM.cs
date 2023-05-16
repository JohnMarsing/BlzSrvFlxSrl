namespace BlzSrvFlxSrl.Features.SpecialEvents;

public record PageHeaderVM
{
	public string? Title { get; init; } // Index / Add / Edit / Display
	public string? Icon { get; init; }
	public string? Color { get; init; }
	public int Id { get; init; }

	/*
	This doesn't work they way I want it to.

	<sup>@State!.Value.PageHeaderVM!Suffix</sup>

	public string Suffix
	{
		get
		{
			return $"{(Id != 0 ? $"Id: {Id}" : "")}";
		}
	}
	*/
}
