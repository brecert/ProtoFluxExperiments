using System.Diagnostics;
using ProtoFluxCompiler.Compiler;
using ProtoFluxCompiler.Nodes;
using ProtoFluxCompiler.Tests;

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

var group = Test1.BibleMarkGroup();
ExternalCall<C>? call = null;
var getValues = NodeGroupCompiler.Compile(group);
getValues((a, b) => { call = (ExternalCall<C>)a; });

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