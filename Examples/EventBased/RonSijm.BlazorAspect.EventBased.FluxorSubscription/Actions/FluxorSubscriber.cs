namespace RonSijm.BlazorAspect.EventBased.FluxorSubscription.Actions;

public static class FluxorComponentSubscriber
{
    private static readonly MethodInfo StateUpdateMethod;
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;

    static FluxorComponentSubscriber()
    {
        StateUpdateMethod = typeof(ComponentBase).GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public static void Subscribe(ComponentBase component)
    {
        var stateProperties = component.GetType().GetProperties(Flags).Where(t => typeof(IStateChangedNotifier).IsAssignableFrom(t.PropertyType));

        Console.WriteLine("Log for demo purposes:");
        Console.WriteLine($"Wiring component {component}");

        foreach (var propertyInfo in stateProperties)
        {
            var value = propertyInfo.GetValue(component);

            if (value is IStateChangedNotifier state)
            {
                state.StateChanged += (sender, args) => StateUpdateMethod.Invoke(component, null);
            }
        }
    }
}