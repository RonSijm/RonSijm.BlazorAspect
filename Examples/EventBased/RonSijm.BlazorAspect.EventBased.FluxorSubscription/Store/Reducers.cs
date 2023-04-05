namespace RonSijm.BlazorAspect.EventBased.FluxorSubscription.Store;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
public static class Reducers
{
    [ReducerMethod]
    public static CounterState ReduceIncrementCounterAction(CounterState state, IncrementCounterAction action) => new(clickCount: state.ClickCount + 1);
}