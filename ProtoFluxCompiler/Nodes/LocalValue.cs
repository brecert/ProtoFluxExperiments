using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class LocalValue<T> : INode, IVariable<T> where T : unmanaged
{
    private T value = default;

    [Output]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Value() => value;


    public T Read() => value;

    public bool Write(T value)
    {
        this.value = value;
        return true;
    }
}
