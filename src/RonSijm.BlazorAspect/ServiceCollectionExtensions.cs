using Microsoft.Extensions.Logging;
using RonSijm.BlazorAspect.Models;

namespace RonSijm.BlazorAspect;

// ReSharper disable UnusedType.Global - Justification: Shipped in public API
// ReSharper disable UnusedMember.Global - Justification: Shipped in public API
// ReSharper disable once MemberCanBePrivate.Global - Justification: Shipped in public API
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds an aspect after the ComponentBase is loaded completely.
    /// </summary>
    public static void UseAspect(this Func<Type, bool> criteria, Expression<Action<ComponentBase>> action)
    {
        var aspect = new ComponentBaseAspect(action);
        AspectActivation.Instance.OnCompleteAspects.Add((criteria, aspect));
    }

    public static Func<Type, bool> WhenAssignableFrom(this IServiceCollection services, Type type)
    {
        var criteria = new Func<Type, bool>(type.IsAssignableFrom);
        return WhenType(services, criteria);
    }

    public static IServiceProvider EnableComponentResolveLogging(this IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<AspectActivation>>();
        AspectActivation.Instance.Logger = logger;

        return services;
    }

    public static Func<Type, bool> WhenType(this IServiceCollection services, Func<Type, bool> criteria)
    {
        RegisterIfNotRegistered(services);

        return criteria;
    }

    private static void RegisterIfNotRegistered(IServiceCollection services)
    {
        // ReSharper disable once InvertIf - Justification: Singleton pattern
        if (!AspectActivation.InstanceLazy.IsValueCreated)
        {
            lock (typeof(AspectActivation))
            {
                if (!AspectActivation.InstanceLazy.IsValueCreated)
                {
                    services.AddSingleton<IComponentActivator>(AspectActivation.Instance);
                }
            }
        }
    }
}