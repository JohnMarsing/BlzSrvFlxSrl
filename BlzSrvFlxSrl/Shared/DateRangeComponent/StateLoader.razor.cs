using Microsoft.AspNetCore.Components;
using System;

namespace BlzSrvFlxSrl.Shared.DateRangeComponent;

public partial class StateLoader
{
    [Inject] private IState<State>? DateRangeComponentState { get; set; }
    [Inject] public IDispatcher? Dispatcher { get; set; }
    [Inject] public ILogger<StateLoader>? Logger { get; set; }

    //protected override void OnInitialized()
    // {
    //	Logger!.LogDebug(string.Format("Inside {0}", nameof(StateLoader) + "!" + nameof(OnInitializedAsync)));
    //	base.OnInitialized();
    //   SubscribeToAction<DateRangeComponent.PersistDateRange_Action>(action => PersistState_StateLoader());
    // }

    protected override void OnAfterRender(bool firstRender)
    {
        Logger!.LogDebug(string.Format("Inside {0}; firstRender:{1}", nameof(StateLoader) + "!" + nameof(OnAfterRender), firstRender));
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            //Dispatcher.Dispatch(new CounterLoadStateAction());
            Dispatcher!.Dispatch(new DateRangeComponent.LoadDateRange_Action());
        }
    }

    private void PersistState_StateLoader()
    {
        Dispatcher!.Dispatch(new DateRangeComponent.PersistDateRange_Action(DateRangeComponentState!.Value));
    }
}
