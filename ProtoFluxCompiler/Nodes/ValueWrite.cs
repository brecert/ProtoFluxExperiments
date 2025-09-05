using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;
using Impulse = System.Action;

namespace ProtoFluxCompiler.Nodes;

[Node]
public sealed class ValueWrite<T> : INode where T : unmanaged
{
    [Reference]
    public IVariable<T>? Variable;

    [Impulse]
    public Impulse? OnWritten;

    [Impulse]
    public Impulse? OnFail;

    [Operation]
    [ProtoFluxName("*")]
    public void Run(
        [Input] T Value
    )
    {
        if (Variable != null && Variable.Write(Value))
        {
            OnWritten?.Invoke();
        }
        else
        {
            OnFail?.Invoke();
        }
    }
}

