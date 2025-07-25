using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;
using Impulse = System.Action;

namespace ProtoFluxCompiler.Nodes;

// TODO: Implement a manual conversion function between nodes so generics aren't needed for the type itself
[Node]
public sealed class ExternalCall<_> : INode
{
    [Impulse]
    public Impulse? Target;

    public void Execute()
    {
        Target?.Invoke();
    }
}
