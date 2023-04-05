namespace RonSijm.BlazorAspect.EventBased.FluxorSubscription.Store;

[FeatureState]
public class CounterState
{
    public int ClickCount { get; }

    // ReSharper disable once UnusedMember.Local - Required for creating initial state
    private CounterState()
    {

    }

    public CounterState(int clickCount)
    {
        ClickCount = clickCount;
    }
}