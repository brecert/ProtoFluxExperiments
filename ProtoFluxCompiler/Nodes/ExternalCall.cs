using ProtoFluxCompiler.Attributes;
using ProtoFluxCompiler.Core;
using Impulse = System.Action;

namespace ProtoFluxCompiler.Nodes;

// Initialization
// var s0 = new ExternalCall();
// var n0 = new ValueConstant<int>
// var n1 = new ValueConstant<int>(0);
// var n2 = new ValueConstant<int>(1);
// var n3 = new ValueAdd<int>();
// var n4 = new ValueLocal();
// var n5 = new ValueWrite<int>();

// References
// n5.Variable = n4;

// Expressions
// var b0 = () =>
//   var o1 = n1.Value();
//   var o2 = n2.Value();
//   var o3 = n3.Value(o1, o2);
//   var op = n5.Run(o3);

// Control Flow
// s0.Target = b0
// // n5.OnWritten = ...
// // n5.OnFail = ...


[Node]
public sealed class ExternalCall : INode
{
    [Impulse]
    public Impulse? Target;

    public void Execute()
    {
        Target?.Invoke();
    }
}
