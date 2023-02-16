using BlzSrvFlxSrl.Features.TestLocalStorageDateRange;
using Microsoft.AspNetCore.Components;

namespace BlzSrvFlxSrl.Features.Profile;

public partial class ThemeToggle
{
	[Parameter] public bool TestTryCatch { get; set; } = false;
	
	private Status _status = Status.Loaded;
	private string? _errorMessage;

	private void Toggle()
	{
		Service?.ToggleDarkMode();
	}

	private async Task ToggleAsync()
	{
		//string inside = "namespace:[" + nameof(ThemeToggle) + "]!" + nameof(Toggle) + "!" + nameof(ToggleAsync);
		await Task.Delay(900);

		try
		{
			_status = Status.Loading;
			// get data from local settings
			//throw new DivideByZeroException();
			//Service?.ToggleDarkMode();
			_status = Status.Loaded;
		}
		catch (Exception ex)
		{
			//Logger!.LogError(ex, string.Format("...Inside catch of {0}", inside));
			//_errorMessage = $"Error! {inside}";
			_errorMessage = $"Error! Message: {ex.Message}";
			_status = Status.Error;
		}
	}

}