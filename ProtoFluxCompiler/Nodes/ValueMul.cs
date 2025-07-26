using System.Runtime.CompilerServices;
using Elements.Core;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

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

