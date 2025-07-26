using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class ToUTF16 : INode
{
    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Value(
        [Input] char Character
    ) => Character;
}

