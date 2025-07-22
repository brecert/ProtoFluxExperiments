using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class ValueConstant<T>(T value) : INode where T : unmanaged
{
    // [Output]
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Value = value;
}
