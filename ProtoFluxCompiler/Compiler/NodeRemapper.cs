using DotNetUtils.Extensions;

namespace ProtoFluxCompiler.Compiler;

public class NodeRemapper
{
    public static readonly Dictionary<Type, Type> TypeMap = new()
    {
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Operators.ValueAdd<>), typeof(ProtoFluxCompiler.Nodes.ValueAdd<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.ExternalCall<>), typeof(ProtoFluxCompiler.Nodes.ExternalCall<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.LocalValue<>), typeof(ProtoFluxCompiler.Nodes.LocalValue<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.ValueConstant<>), typeof(ProtoFluxCompiler.Nodes.ValueConstant<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.ObjectConstant<>), typeof(ProtoFluxCompiler.Nodes.ObjectConstant<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.ValueWrite<>), typeof(ProtoFluxCompiler.Nodes.ValueWrite<>)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Strings.StringLength), typeof(ProtoFluxCompiler.Nodes.StringLength)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.For), typeof(ProtoFluxCompiler.Nodes.For)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Strings.Characters.GetCharacter), typeof(ProtoFluxCompiler.Nodes.GetCharacter)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Strings.Characters.ToUTF16), typeof(ProtoFluxCompiler.Nodes.ToUTF16)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Casts.Cast_int_To_ulong), typeof(ProtoFluxCompiler.Nodes.Cast_int_To_ulong)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Operators.XOR_Ulong), typeof(ProtoFluxCompiler.Nodes.XOR_Ulong)},
        {typeof(ProtoFlux.Runtimes.Execution.Nodes.Operators.ValueMul<>), typeof(ProtoFluxCompiler.Nodes.ValueMul<>)},
    };

    public static Type RemapType(Type nodeType)
    {
        if (nodeType.TryGetGenericTypeDefinition(out var genericTypeDefinition))
        {
            var genericArguments = nodeType.GenericTypeArguments;
            return TypeMap[genericTypeDefinition].MakeGenericType(genericArguments);
        }
        else
        {
            return TypeMap[nodeType];
        }
    }
}