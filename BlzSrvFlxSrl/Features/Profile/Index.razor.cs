using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BlzSrvFlxSrl.Features.Profile;

public partial class Index
{
	[Inject] public ILogger<Index>? Logger { get; set; }
	[Inject] public Service? Svc { get; set; }
	//[Inject] public Service Svc { get; set; }

	private Preferences _preferences = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		string inside = "namespace:[" + nameof(Profile) + "]!" + nameof(Index) + "!" + nameof(OnAfterRenderAsync);
		Logger!.LogDebug(string.Format("Inside {0}; firstRender:{1}", inside, firstRender));

		if (firstRender)
		{
			//Svc.OnChange += ServiceOnOnChange();
			//Svc!.OnChange += ServiceOnOnChange();
			//_preferences = await Profile.Service.GetPreferences();
			_preferences = await Svc.GetPreferences();
			StateHasChanged();
		}
	}


	private void ServiceOnOnChange(Preferences newPreferences)
	{
		_preferences = newPreferences;
		StateHasChanged();
	}

	//private Preferences ServiceOnOnChange(Preferences newPreferences)
	//{
	//	_preferences = newPreferences;
	//	StateHasChanged();
	//	return _preferences;
	//}

	public void Dispose()
	{
		Svc!.OnChange -= ServiceOnOnChange;
	}

}

/*

### WASM solution

```csharp
	protected override async Task OnInitializedAsync()
	{
		_preferences = await Service.GetPreferences();
	}
```

## Server solution 
>	You can't use `OnInitializedAsync` because that code runs on the server as part of a prerendering phase, 
	and the server doesn’t have access to your browser’s local storage via javascript

> explicitly instruct Blazor that `StateHasChanged` so it knows it needs to render
	(typically it wouldn’t need to render again after rendering has just finished, hence the need to be explicit).

```csharp
	
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		_preferences = await Service.GetPreferences();
		if (firstRender)
		{
			_preferences = await Service.GetPreferences();
			StateHasChanged();
		}
	}

```

- [Resource](https://jonhilton.net/blazor-tailwind-dark-mode-local-storage/)



*/