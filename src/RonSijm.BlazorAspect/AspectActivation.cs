using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RonSijm.BlazorAspect.Models;

namespace RonSijm.BlazorAspect;

public sealed class AspectActivation : IComponentActivator
{
    internal static readonly Lazy<AspectActivation> InstanceLazy = new(() => new AspectActivation());
    internal static AspectActivation Instance => InstanceLazy.Value;
    internal ILogger Logger { get; set; } = NullLogger.Instance;

    private readonly FieldInfo _renderFragmentFieldInfo;

    internal readonly List<(Func<Type, bool> Criteria, ComponentBaseAspect Action)> OnCompleteAspects = new();

    private AspectActivation()
    {
        _renderFragmentFieldInfo = typeof(ComponentBase).GetField("_renderFragment", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
    {
        if (componentType == null)
        {
            Logger.LogWarning("Cannot create instance when type is null");
            throw new ArgumentNullException(nameof(componentType));
        }

        Logger.LogDebug("Creating instance {componentType}", componentType);

        if (!typeof(IComponent).IsAssignableFrom(componentType))
        {
            Logger.LogError("Cannot create instance when type is not of type IComponent");
            throw new ArgumentException($"The type {componentType.FullName} does not implement {nameof(IComponent)}.", nameof(componentType));
        }

        var instance = (IComponent)Activator.CreateInstance(componentType);

        if (instance is not ComponentBase component)
        {
            Logger.LogDebug("Type {type} is not ComponentBase, so not attempting to applying any extra behavior", componentType);
            return instance;
        }

        WireEventBasedAspects(componentType, component);

        return component;
    }

    private void WireEventBasedAspects(Type componentType, ComponentBase component)
    {
        // We check if there's any event to attach,
        // Because if there aren't we don't have to assign anything to the delegate
        List<ComponentBaseAspect> aspectList = null;

        foreach (var aspect in OnCompleteAspects.Where(aspect => aspect.Criteria.Invoke(componentType)))
        {
            aspectList ??= new List<ComponentBaseAspect>();
            aspectList.Add(aspect.Action);
        }

        if (aspectList == null)
        {
            return;
        }

        var renderFragmentValue = _renderFragmentFieldInfo.GetValue(component) as RenderFragment;
        foreach (var action in aspectList)
        {
            Logger.LogDebug("Applying Aspect '{action}' to {componentType}", action, componentType);

            // Render Fragment is a delegate void unfortunately, so we cannot have async aspects
            renderFragmentValue += (_ => { action.Invoke(component); });
        }

        _renderFragmentFieldInfo.SetValue(component, renderFragmentValue);
    }
}