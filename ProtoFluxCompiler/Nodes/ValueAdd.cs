using System.Runtime.CompilerServices;
using Elements.Core;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class ValueAdd<T> : INode where T : unmanaged
{
    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Value(
        [Input] T A,
        [Input] T B
    ) => Coder<T>.Add(A, B);
}

[Node]
public sealed class StringLength : INode
{
    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Value(
        [Input] string? A
    ) => A?.Length ?? 0;
}

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

[Node]
public sealed class ValueMul<T> : INode where T : unmanaged
{
    static T GetDefaultOfA() => Coder<T>.Identity;
    static T GetDefaultOfB() => Coder<T>.Identity;

    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Value(
        [Input(@default: nameof(GetDefaultOfA))]
        T A,
        [Input(@default: nameof(GetDefaultOfB))]
        T B
    ) => Coder<T>.Mul(A, B);
}

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

[Node]
public sealed class XOR_Ulong : INode
{
    [Output]
    [ProtoFluxName("*")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Value(
        [Input] ulong A,
        [Input] ulong B
    ) => A ^ B;
}

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

