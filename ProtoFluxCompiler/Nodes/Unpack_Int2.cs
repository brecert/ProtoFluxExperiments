using System.Runtime.CompilerServices;
using Elements.Core;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class Unpack_Int2 : INode
{
    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int X([Input] int2 V) => V.x;

    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Y([Input] int2 V) => V.y;
}

