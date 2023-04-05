namespace RonSijm.BlazorAspect.Models;

public sealed class ComponentBaseAspect : Aspect<ComponentBase>
{
    public ComponentBaseAspect(Expression<Action<ComponentBase>> actionExpression) : base(actionExpression)
    {
    }
}