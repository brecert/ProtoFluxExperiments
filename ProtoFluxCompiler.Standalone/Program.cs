using System.Diagnostics;
using ProtoFluxCompiler.Compiler;
using ProtoFluxCompiler.Nodes;
using ProtoFluxCompiler.Tests;
using ProtoFluxCompiler.Tests.Data;
using C = ProtoFlux.Runtimes.Execution.ExecutionContext;

var text = File.ReadAllText(@"D:\bree\Code\local\Resonite\BreeFluxTesting\bible.txt");
// Console.WriteLine(text.Length);
// var run = BibleMarkTesting.CompiledBibleMark(text);
// void bench()
// {
//     var sw = new Stopwatch();
//     sw.Start();
//     run();
//     sw.Stop();
//     Console.WriteLine(sw.Elapsed.TotalMilliseconds);
// }
// bench();
// bench();
// bench();
// bench();
// bench();
// bench();
// bench();
// bench();
// bench();
// bench();

var group = NodeGroupTemplates.XorHashGroup("");
// ExternalCall<C>? call = null;
// var getValues = NodeGroupCompiler.CompileForTesting(group);


var getValues = (Func<ProtoFluxCompiler.Core.INode[]>)(() => //INode[]
{
    ExternalCall<C> n0 = null;
    ValueConstant<ulong> n1 = null;
    ValueConstant<ulong> n2 = null;
    ObjectConstant<string> n3 = null;
    StringLength n4 = null;
    For n5 = null;
    GetCharacter n6 = null;
    ToUTF16 n7 = null;
    ValueMul<ulong> n8 = null;
    Cast_int_To_ulong n9 = null;
    XOR_Ulong n10 = null;
    ValueWrite<ulong> n11 = null;
    ValueWrite<ulong> n12 = null;
    ValueWrite<ulong> n13 = null;
    LocalValue<ulong> n14 = null;
    // Action b0 = null;
    // Action b1 = null;
    // Action b2 = null;
    // Action b3 = null;
    ProtoFluxCompiler.Core.INode[] result = null;
    n0 = new();
    n1 = new()
    {
        value = 1099511628211
    };
    n2 = new()
    {
        value = 14695981039346656037
    };
    n3 = new()
    {
        value = text
    };
    n4 = new();
    n5 = new();
    n6 = new();
    n7 = new();
    n8 = new();
    n9 = new();
    n10 = new();
    n11 = new();
    n12 = new();
    n13 = new();
    n14 = new();
    n11.Variable = n14;
    n12.Variable = n14;
    n13.Variable = n14;
    void b0() //void
    {
        string o0 = null;
        int o1 = default;
        o0 = n3.Value();
        o1 = StringLength.Value(o0);
        n5.Run(o1, default);
    };
    void b1() //void
    {
        ulong o0_1 = default;
        o0_1 = n2.Value();
        n11.Run(o0_1);
    };
    void b2()//void
    {
        ulong o0_2 = default;
        ulong o1_1 = default;
        ulong o2 = default;
        o0_2 = n14.Value();
        o1_1 = n1.Value();
        o2 = n8.Value(
            o1_1,
            o0_2);
        n12.Run(o2);
    };
    void b3()
    {
        string o0_3 = null;
        int o1_2 = default;
        int o2_1 = default;
        char o3 = default;
        int o4 = default;
        ulong o5 = default;
        ulong o6 = default;
        ulong o7 = default;
        o0_3 = n3.Value();
        o1_2 = StringLength.Value(o0_3);
        o2_1 = n5.Iteration();
        o3 = GetCharacter.Value(
            o0_3,
            o2_1);
        o4 = ToUTF16.Value(o3);
        o5 = Cast_int_To_ulong.Value(o4);
        o6 = n14.Value();
        o7 = XOR_Ulong.Value(
            o6,
            o5);
        n13.Run(o7);
    };
    n0.Target = b0;
    n5.LoopStart = b1;
    n5.LoopIteration = b2;
    // n5.LoopEnd = () =>
    // {
    //     Console.WriteLine(n14.Value());
    // };
    n12.OnWritten = b3;
    result = [
        n0,
        n1,
        n2,
        n3,
        n4,
        n5,
        n6,
        n7,
        n8,
        n9,
        n10,
        n11,
        n12,
        n13,
        n14,
    ];
    return result;
});

var call = (ExternalCall<C>)getValues().First(n => n is ExternalCall<C>);
// getValues((a, b) => { call = (ExternalCall<C>)a; });


bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();
bench();

void bench()
{
    var sw = new Stopwatch();
    sw.Start();
    call.Execute();
    sw.Stop();
    Console.WriteLine(sw.Elapsed.TotalMilliseconds);
}