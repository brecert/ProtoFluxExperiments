using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class ObjectConstant<T>(T value) : INode
{
    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Value() => value;
}
