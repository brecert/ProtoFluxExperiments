namespace ProtoFluxCompiler.Compiler;

using ProtoFluxCompiler.Nodes;
using ProtoFluxCompiler.Tests.Data;
using C = ProtoFlux.Runtimes.Execution.ExecutionContext;
using R = ProtoFlux.Runtimes.Execution.ExecutionRuntime<ProtoFlux.Runtimes.Execution.ExecutionContext>;

public sealed class CompilerTests
{
    // TODO: automatically compare ProtoFlux and Compiled results.
    [Fact]
    public static void ValueAddGroup()
    {
        var group = NodeGroupTemplates.ValueAddGroup(5, 3);
        var createInstance = NodeGroupCompiler.CompileForTesting(group);
        var nodes = createInstance();
        var call = (ExternalCall<C>)nodes[0];
        var local = (LocalValue<int>)nodes[^1];
        call.Execute();
        Assert.Equal(8, local.Value());
    }

    [Theory]
    [InlineData("", 14695981039346656037)]
    [InlineData("a", 12638153115695167422)]
    [InlineData("example text", 10961004045882370890)]
    public static void XorHashGroup(string data, ulong result)
    {
        var group = NodeGroupTemplates.XorHashGroup(data);
        var createInstance = NodeGroupCompiler.CompileForTesting(group);
        var nodes = createInstance();
        var call = (ExternalCall<C>)nodes[0];
        var local = (LocalValue<ulong>)nodes[^1];
        call.Execute();
        Assert.Equal(result, local.Value());
    }
}