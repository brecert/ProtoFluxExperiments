using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;
using Impulse = Action;

[Node]
public sealed class DataModelBooleanToggle : INode
{
    bool value = default;

    [Impulse]
    public Impulse? OnSet;

    [Impulse]
    public Impulse? OnReset;

    [Operation]
    public void Set()
    {
        value = true;
        OnSet?.Invoke();
    }

    [Operation]
    public void Reset()
    {
        value = false;
        OnReset?.Invoke();
    }

    [Operation]
    public void Toggle()
    {
        value = !value;
        if (value) OnSet?.Invoke(); else OnReset?.Invoke();
    }

    [Output]
    public bool Value() => value;
}
