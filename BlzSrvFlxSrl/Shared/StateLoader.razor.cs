using Microsoft.AspNetCore.Components;
using BlzSrvFlxSrl.Shared.Header;

namespace BlzSrvFlxSrl.Shared;

public partial class StateLoader
{
  [Inject] private IState<State>? BibleSearchState { get; set; }
  [Inject] public IDispatcher? Dispatcher { get; set; }

  protected override void OnInitialized()
  {
    base.OnInitialized();


    //SubscribeToAction<BibleSearch_PersistState_Action>(
    //  action => PersistBibleSearchState());

  }

  protected override void OnAfterRender(bool firstRender)
  {
    base.OnAfterRender(firstRender);

    if (firstRender)
    {
      //Dispatcher.Dispatch(new CounterLoadStateAction());
      //Dispatcher.Dispatch(new WeatherLoadStateAction());
    }
  }

  private void PersistBibleSearchState()
  {
    //Dispatcher!.Dispatch(new BibleSearch_PersistState_Action(
    //  BibleSearchState!.Value));
  }
}
