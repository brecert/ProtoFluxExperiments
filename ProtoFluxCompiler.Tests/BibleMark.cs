using System.Diagnostics;
using Elements.Core;
using ProtoFluxCompiler.Nodes;

namespace ProtoFluxCompiler.Tests;


[TestClass]
public sealed class BibleMarkTesting
{

    [TestMethod]
    public void TestCompiledBibleMark()
    {
        var text = File.ReadAllText(@"D:\bree\Code\local\Resonite\BreeFluxTesting\bible.txt");
        Console.WriteLine(text.Length);
        var run = CompiledBibleMark(text);
        void bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            run();
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
        }
        bench();
        bench();
        bench();
        bench();
        bench();
    }

    public static Action CompiledBibleMark(string input)
    {
        var n0 = new ExternalCall<dummy>();
        var n1 = new ValueConstant<ulong>
        {
            value = 14695981039346656037UL
        };
        var n2 = new ValueConstant<ulong>
        {
            value = 1099511628211UL
        };
        var n3 = new ObjectConstant<string>
        {
            value = input
        };
        var n4 = new StringLength();
        var n5 = new For();
        var n6 = new GetCharacter();
        var n7 = new ToUTF16();
        var n8 = new ValueMul<ulong>();
        var n9 = new Cast_int_To_ulong();
        var n10 = new XOR_Ulong();
        var n11 = new ValueWrite<ulong>();
        var n12 = new ValueWrite<ulong>();
        var n13 = new ValueWrite<ulong>();
        var n14 = new LocalValue<ulong>();

        n11.Variable = n14;
        n12.Variable = n14;
        n13.Variable = n14;

        void b0()
        {
            var v0 = n3.Value();
            var v1 = n4.Value(v0);
            n5.Run(v1, false);
        }

        void b1()
        {
            var v0 = n1.Value();
            n11.Run(v0);
        }

        void b2()
        {
            var v0 = n14.Value();
            var v1 = n2.Value();
            var v2 = n8.Value(v0, v1);
            n12.Run(v2);
        }

        void b3()
        {
            var v0 = n3.Value();
            var v1 = n5.Iteration();
            var v2 = n6.Value(v0, v1);
            var v3 = n7.Value(v2);
            var v4 = Cast_int_To_ulong.Value(v3);
            var v5 = n14.Value();
            var v6 = n10.Value(v5, v4);
            n13.Run(v6);
        }

        n0.Target = b0;
        n5.LoopStart = b1;
        n5.LoopIteration = b2;
        n12.OnWritten = b3;

        void Execute()
        {
            n0.Execute();
            Console.WriteLine(n14.Value());
        }

        return Execute;
    }
}
