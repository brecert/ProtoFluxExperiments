using System.Runtime.CompilerServices;
using Elements.Core;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;
using Impulse = System.Action;

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Value(
        [Input] string? A
    ) => A?.Length ?? 0;
}

[Node]
public sealed class GetCharacter : INode
{
    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char Value(
        [Input] string? str,
        [Input] int index
    ) =>
        str is not null && index >= 0 && index < str.Length
            ? str[index]
            : '\0';
}

[Node]
public sealed class ToUTF16 : INode
{
    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Value(
        [Input] char character
    ) => character;
}


[Node]
public sealed class For : INode
{
    int index = 0;

    [Impulse]
    public Impulse? LoopStart;

    [Impulse]
    public Impulse? LoopIteration;

    [Impulse]
    public Impulse? LoopEnd;

    [Operation]
    public void Run(
        [Input] int count,
        [Input] bool reverse
    )
    {
        LoopStart?.Invoke();
        if (!reverse)
        {
            for (int i = 0; i < count; i++)
            {
                index = i;
                if(LoopIteration != null) LoopIteration();

            }

        }
        else
        {
            for (int i = count - 1; i >= 0; i--)
            {
                index = i;
                LoopIteration?.Invoke();
            }
        }
        index = 0;
        LoopEnd?.Invoke();
    }

    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Index() => index;
}

[Node]
public sealed class ValueMul<T> : INode where T : unmanaged
{
    static T GetDefaultOfA() => Coder<T>.Identity;
    static T GetDefaultOfB() => Coder<T>.Identity;

    [Output]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Value(
        [Input] int Input
    ) => (ulong)Input;
}

[Node]
public sealed class XOR_Ulong : INode
{
    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Value(
        [Input] ulong A,
        [Input] ulong B
    ) => A ^ B;
}