using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class ObjectConstant<T> : INode
{
    [Constant]
    [ProtoFluxName("Value")]
    public T value;

    public ObjectConstant() { }

    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Value() => value;
}
