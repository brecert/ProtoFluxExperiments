using System.Diagnostics;
using Elements.Core;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;
using ProtoFlux.Runtimes.Execution.Nodes;
using ProtoFlux.Runtimes.Execution.Nodes.Casts;
using ProtoFlux.Runtimes.Execution.Nodes.Operators;
using ProtoFlux.Runtimes.Execution.Nodes.Strings;
using ProtoFlux.Runtimes.Execution.Nodes.Strings.Characters;
using ProtoFluxCompiler.Compiler;

using C = ProtoFlux.Runtimes.Execution.ExecutionContext;
using R = ProtoFlux.Runtimes.Execution.ExecutionRuntime<ProtoFlux.Runtimes.Execution.ExecutionContext>;

namespace ProtoFluxCompiler.Tests;


[TestClass]
public sealed class Test1
{

    [TestMethod]
    public void TestMethod1()
    {
        var group = ValueAddGroup();
        var compile = NodeGroupCompiler.Compile(group);
        compile((a, b) =>
        {
            var A = (Nodes.ExternalCall<C>)a;
            var B = (Nodes.LocalValue<int>)b;
            Debug.WriteLine(A);
            Debug.WriteLine(B);
            A.Execute();
            Debug.WriteLine(B.Value());
        });
        // var (call, local) = compile();
        // Debug.WriteLine(local.Read());
        // call.Execute();
        // Debug.WriteLine(local.Read());

        // Reflow.BuildFlowTable<C>(group);
        // Debug.WriteLine(Reflow.RemapGroup<C>(group).ToString());
    }

    static NodeGroup ValueAddGroup()
    {
        var group = new NodeGroup("ValueAdd Group");
        var runtime = group.AddRuntime<R>();

        var call = runtime.AddNode<ExternalCall<C>>();
        var a = runtime.AddNode<ValueConstant<int>>();
        var b = runtime.AddNode<ValueConstant<int>>();
        var add = runtime.AddNode<ValueAdd<int>>();
        var write = runtime.AddNode<ValueWrite<int>>();
        var store = runtime.AddNode<LocalValue<int>>();
        a.Value = 3;
        b.Value = 4;

        call.Target.Target = write;
        add.A.Source = a;
        add.B.Source = b;
        write.Value.Source = add;
        write.Variable.Target = store;

        runtime.Rebuild();
        return group;
    }

    static NodeGroup BibleMarkGroup()
    {
        var group = new NodeGroup("BibleMark Group");
        var runtime = group.AddRuntime<R>();

        var start = runtime.AddNode<ExternalCall<C>>();

        var mulInput = runtime.AddNode<ValueConstant<ulong>>();
        var startInput = runtime.AddNode<ValueConstant<ulong>>();
        var stringInput = runtime.AddNode<ObjectConstant<string>>();
        var stringLength = runtime.AddNode<StringLength>();
        var forLoop = runtime.AddNode<For>();
        var getCharacter = runtime.AddNode<GetCharacter>();
        var toUTF16 = runtime.AddNode<ToUTF16>();
        var resultMultiply = runtime.AddNode<ValueMul<ulong>>();
        var cast = runtime.AddNode<Cast_int_To_ulong>();
        var resultXor = runtime.AddNode<XOR_Ulong>();
        var writeResult0 = runtime.AddNode<ValueWrite<ulong>>();
        var writeResult1 = runtime.AddNode<ValueWrite<ulong>>();
        var writeResult2 = runtime.AddNode<ValueWrite<ulong>>();
        var resultLocal = runtime.AddNode<LocalValue<ulong>>();

        // stringInput.Value = System.IO.File.ReadAllText "bible.txt";

        start.Target.Target = forLoop;

        writeResult0.Variable.Target = resultLocal;
        writeResult2.Variable.Target = resultLocal;
        writeResult1.Variable.Target = resultLocal;

        startInput.Value = 14695981039346656037UL;
        writeResult0.Value.Source = startInput;
        forLoop.LoopStart.Target = writeResult0;

        stringLength.A.Source = stringInput;
        getCharacter.Str.Source = stringInput;
        forLoop.Count.Source = stringLength;
        getCharacter.Index.Source = forLoop.Iteration;
        toUTF16.Character.Source = getCharacter;
        cast.Input.Source = toUTF16;

        mulInput.Value = 1099511628211UL;
        resultMultiply.A.Source = mulInput;
        resultMultiply.B.Source = resultLocal;
        writeResult1.Value.Source = resultMultiply;
        forLoop.LoopIteration.Target = writeResult1;

        resultXor.A.Source = resultLocal;
        resultXor.B.Source = cast;

        writeResult1.OnWritten.Target = writeResult2;
        writeResult2.Value.Source = resultXor;

        runtime.Rebuild();
        return group;
    }
}
