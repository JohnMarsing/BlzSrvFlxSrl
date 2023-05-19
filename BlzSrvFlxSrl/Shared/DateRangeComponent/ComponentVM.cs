using Newtonsoft.Json;

namespace BlzSrvFlxSrl.Shared.DateRangeComponent;

public class ComponentVM
{
	[JsonProperty] public DateTimeOffset? DateBegin { get; private set; }
	[JsonProperty] public DateTimeOffset? DateEnd { get; private set; }
	//[JsonProperty] public int TimeToLiveInSeconds { get; set; } = 60;
	//[JsonProperty] public DateTime LastAccessed { get; set; } = DateTime.Now;
	
	public ComponentVM()
	{
		DateBegin = DateTime.UtcNow.AddMonths(-6);
		DateEnd = DateTime.UtcNow.AddMonths(+6);
	}
}
