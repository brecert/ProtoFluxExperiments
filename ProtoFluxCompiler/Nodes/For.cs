using System.Runtime.CompilerServices;
using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;
using Impulse = System.Action;

namespace ProtoFluxCompiler.Nodes;

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
    [ProtoFluxName("*")]
    public void Run(
        [Input] int Count,
        [Input] bool Reverse
    )
    {
        LoopStart?.Invoke();
        if (!Reverse)
        {
            for (int i = 0; i < Count; i++)
            {
                index = i;
                LoopIteration?.Invoke();

            }
        }
        else
        {
            for (int i = Count - 1; i >= 0; i--)
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
    public int Iteration() => index;
}
