using System.Diagnostics;
using ProtoFluxCompiler.Tests;

var text = File.ReadAllText(@"D:\bree\Code\local\Resonite\BreeFluxTesting\bible.txt");
Console.WriteLine(text.Length);
var run = BibleMarkTesting.CompiledBibleMark(text);
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
bench();
bench();
bench();
bench();
bench();
