using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class GetCharacter : INode
{
    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char Value(
        [Input] string? Str,
        [Input] int Index
    ) =>
        Str is not null && Index >= 0 && Index < Str.Length
            ? Str[Index]
            : '\0';
}

