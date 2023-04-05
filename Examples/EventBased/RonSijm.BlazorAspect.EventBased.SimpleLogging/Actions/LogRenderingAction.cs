using Microsoft.AspNetCore.Components;

namespace RonSijm.BlazorAspect.EventBased.SimpleLogging.Actions;

public static class LogRenderingAction
{
    public static void Log(ComponentBase component)
    {
        Console.WriteLine($"[{typeof(LogRenderingAction).FullName}] - (Injected Aspect) Rendered component: {component}");
    }
}