namespace RonSijm.BlazorAspect.Models;

/// <summary>
/// Model that encapsulates a function from an expression
/// For the sake of logging, and understanding which function has been added.
/// </summary>
public abstract class Aspect<T1>
{
    private readonly Expression<Action<T1>> _actionExpression;
    private readonly Action<T1> _action;

    protected Aspect(Expression<Action<T1>> actionExpression)
    {
        _actionExpression = actionExpression;
        _action = actionExpression.Compile();
    }

    public void Invoke(T1 arg1)
    {
        _action.Invoke(arg1);
    }

    public override string ToString()
    {
        return _actionExpression.ToString();
    }
}