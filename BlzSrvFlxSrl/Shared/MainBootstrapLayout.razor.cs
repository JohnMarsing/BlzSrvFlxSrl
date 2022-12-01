namespace BlzSrvFlxSrl.Shared;
/*
  Copied from: MainLayout.razor
    - BlazorWithRedux2\BlazorWithFluxor\Client\Shared\ 
 */
public partial class MainBootstrapLayout
{
  protected override void OnInitialized()
  {
    base.OnInitialized();
    //Dispatcher.Dispatch(new CounterHubStartAction());
  }
}
