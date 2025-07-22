// using DotNetUtils.Extensions;
// using ProtoFlux.Core;
// using ProtoFluxCompiler.Nodes;

// class NodeRemapper
// {
//     static readonly Dictionary<Type, Type> TypeMap = new()
//     {
//         {typeof(ProtoFlux.Runtimes.Execution.Nodes.Operators.ValueAdd<>), typeof(ProtoFluxCompiler.Nodes.ValueAdd<>)},
//         {typeof(ProtoFlux.Runtimes.Execution.Nodes.ExternalCall<>), typeof(ProtoFluxCompiler.Nodes.ExternalCall<>)},
//         {typeof(ProtoFlux.Runtimes.Execution.Nodes.LocalValue<>), typeof(ProtoFluxCompiler.Nodes.LocalValue<>)},
//         {typeof(ProtoFlux.Runtimes.Execution.Nodes.ValueConstant<>), typeof(ProtoFluxCompiler.Nodes.ValueConstant<>)},
//         {typeof(ProtoFlux.Runtimes.Execution.Nodes.ValueWrite<>), typeof(ProtoFluxCompiler.Nodes.ValueWrite<>)},
//     };

//     public static IExpressionNode Map(INode node)
//     {
//         if (node.GetType().TryGetGenericTypeDefinition(out var genericTypeDefinition))
//         {
//             var genericArguments = node.GetType().GenericTypeArguments;
//             return (IExpressionNode)TypeMap[genericTypeDefinition].MakeGenericType(genericArguments);
//         }
//         else
//         {
//             return (IExpressionNode)TypeMap[node.GetType()];
//         }
//     }
// }