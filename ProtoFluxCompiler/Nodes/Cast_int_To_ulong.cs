using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class Cast_int_To_ulong : INode
{
    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Value(
        [Input] int Input
    ) => (ulong)Input;
}

