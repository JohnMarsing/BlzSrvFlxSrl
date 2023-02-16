using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BlzSrvFlxSrl.Features.TestLocalStorageDateRange;

public partial class Index
{
	[Inject] public ILogger<Index>? Logger { get; set; }
	[Inject] public ILocalStorageService? LocalStorage { get; set; }

	private Status _status;
	private string? _errorMessage;
	private const string DateRangePersistenceName = "BlzSrvFlxSrl_DateRange";

	IEnumerable<string> Keys { get; set; } = new List<string>();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		string inside = "namespace:[" + nameof(TestLocalStorageDateRange) + "]!" + nameof(Index) + "!" + nameof(OnAfterRenderAsync);
		Logger!.LogDebug(string.Format("Inside {0}; firstRender:{1}", inside, firstRender));

		Keys = await LocalStorage!.KeysAsync();
		if (firstRender)
		{
		}
	}
	/*
	protected override async Task OnInitializedAsync()
	{
		//base.OnInitializedAsync(); 
		await LoadData();
	}
	*/

	private async Task LoadData()
	{
		string inside = "namespace:[" + nameof(TestLocalStorageDateRange) + "]!" + nameof(Index) + "!" + nameof(LoadData);
		//await Task.Delay(900);
		var state = await LocalStorage!.GetItemAsync<DateRangeVM>("BlzSrvFlxSrl_DateRange");

		try
		{
			_status = Status.Loading;
			// get data from local settings
			//throw new DivideByZeroException();
			_status = Status.Loaded;
		}
		catch (Exception ex)
		{
			Logger!.LogError(ex, string.Format("...Inside catch of {0}", inside));
			_errorMessage = $"Error! {inside}";
			_status = Status.Error;
		}
	}

}
/*


		//var state = await LocalStorage!.GetItemAsync<DateRangeVM>(DateRangePersistenceName);
*/
public enum Status
{
	Loading,
	Loaded,
	Error
}

public class DateRangeVM
{
	[JsonProperty] public DateTimeOffset? DateBegin { get; private set; }
	[JsonProperty] public DateTimeOffset? DateEnd { get; private set; }
	//[JsonProperty] public int TimeToLiveInSeconds { get; set; } = 60;
	//[JsonProperty] public DateTime LastAccessed { get; set; } = DateTime.Now;

	public DateRangeVM()
	{
		DateBegin = DateTime.UtcNow.AddMonths(-6);
		DateEnd = DateTime.UtcNow.AddMonths(+6);
	}
}
