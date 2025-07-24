using System.Diagnostics;
using DotNetUtils.Extensions;
using ProtoFlux.Core;
using ProtoFluxCompiler.Nodes;

public class NodeRemapper
{
    public static readonly Dictionary<Type, Type> TypeMap = new()
    {
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Operators.ValueAdd<>), typeof(ProtoFluxCompiler.Nodes.ValueAdd<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.ExternalCall<>), typeof(ProtoFluxCompiler.Nodes.ExternalCall<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.LocalValue<>), typeof(ProtoFluxCompiler.Nodes.LocalValue<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.ValueConstant<>), typeof(ProtoFluxCompiler.Nodes.ValueConstant<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.ValueWrite<>), typeof(ProtoFluxCompiler.Nodes.ValueWrite<>)},
    };

    public static Type RemapType(Type nodeType)
    {
        Debug.WriteLine(nodeType);
        if (nodeType.TryGetGenericTypeDefinition(out var genericTypeDefinition))
        {
            Debug.WriteLine(genericTypeDefinition);
            var genericArguments = nodeType.GenericTypeArguments;
            Debug.WriteLine(genericArguments.Length);
            return TypeMap[genericTypeDefinition].MakeGenericType(genericArguments);
        }
        else
        {
            return TypeMap[nodeType.GetType()];
        }
    }
}