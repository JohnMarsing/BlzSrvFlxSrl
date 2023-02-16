using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.TestLocalStorage;

public partial class Index
{
	/*
	@inherits Fluxor.Blazor.Web.Components.FluxorComponent

	[Inject] private IState<State>? SpecialEventsState { get; set; }
	[Inject] public IDispatcher? Dispatcher { get; set; }
	*/

	[Inject] public ILogger<Index>? Logger { get; set; }
	[Inject] public ILocalStorageService? localStorage { get; set; }

	string? NameFromLocalStorage { get; set; }
	int ItemsInLocalStorage { get; set; }
	string? Name { get; set; }
	bool ItemExist { get; set; }

	string inside = nameof(Links.TestLocalStorage) + "!" + nameof(Index); // + "!" + nameof(OnAfterRenderAsync);

	IEnumerable<string> Keys { get; set; } = new List<string>();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		Logger!.LogDebug(string.Format("Inside {0}!{1}; firstRender:{2}", inside, nameof(OnAfterRenderAsync), firstRender));
		Keys = await localStorage!.KeysAsync();

		if (firstRender)
		{
			await GetNameFromLocalStorage();
			await GetLocalStorageLength();

			localStorage.Changed += (_, e) =>
			{
				Logger!.LogDebug(string.Format("... Value for key {0} changed from {1} to {2}", e.Key, e.OldValue, e.NewValue));
			};
			await TestTimespan();
			StateHasChanged();
		}
	}

	async Task SaveName()
	{
		await localStorage!.SetItemAsync("name", Name);
		await GetNameFromLocalStorage();
		await GetLocalStorageLength();

		Name = "";

		Keys = await localStorage.KeysAsync();
	}

	async Task GetNameFromLocalStorage()
	{
		NameFromLocalStorage = await localStorage!.GetItemAsync<string>("name");

		if (string.IsNullOrEmpty(NameFromLocalStorage))
		{
			NameFromLocalStorage = "Nothing Saved";
		}
	}

	async Task RemoveName()
	{
		await localStorage!.RemoveItemAsync("name");
		await GetNameFromLocalStorage();
		await GetLocalStorageLength();
	}

	async Task ClearLocalStorage()
	{
		Logger!.LogDebug(string.Format("...Inside {0}; calling {1} ", nameof(ClearLocalStorage), nameof(localStorage.ClearAsync)));

		await localStorage!.ClearAsync();

		Logger!.LogDebug(string.Format("...calling {0} ", nameof(GetNameFromLocalStorage)));
		await GetNameFromLocalStorage();

		Logger!.LogDebug(string.Format("...calling {0} ", nameof(GetLocalStorageLength)));
		await GetLocalStorageLength();
	}

	async Task GetLocalStorageLength()
	{
		Logger!.LogDebug(string.Format("...Inside {0}; calling {1} ", nameof(GetLocalStorageLength), nameof(localStorage.LengthAsync)));
		Console.WriteLine(await localStorage!.LengthAsync());
		ItemsInLocalStorage = await localStorage.LengthAsync();
		ItemExist = await localStorage.ContainKeyAsync("name");
	}

	async Task TestTimespan()
	{
		var timespan = await localStorage!.GetItemAsync<TimeSpan>("timespan");
		if (timespan == TimeSpan.Zero)
		{
			await localStorage.SetItemAsync("timespan", new TimeSpan(0, 15, 0));
			timespan = await localStorage.GetItemAsync<TimeSpan>("timespan");
		}
	}

}