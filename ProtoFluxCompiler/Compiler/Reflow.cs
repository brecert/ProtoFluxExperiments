using System.Text;
using Elements.Core;
using ProtoFlux.Core;
using ProtoFluxCompiler.Collections.Generic;
using ProtoFluxUtils.Elements;
using ProtoFluxUtils.Extensions;

namespace ProtoFluxCompiler.Compiler;

/// <summary>
/// Provides methods for working with and converting node groups into ordered sequences.
/// </summary>
public static class Reflow
{
    /// <summary>
    /// Builds a linear sequence of outputs.
    /// </summary>
    /// <param name="node">The node to build a sequence from.</param>
    /// <param name="set">The collection of already seen elements, ordered by .</param>
    /// <remarks>
    /// The order
    /// </remarks>
    internal static void BuildSequence(INode node, in OrderedPushSet<OutputElement> set)
    {
        foreach (var input in node.AllInputElements())
        {
            var element = input.SourceElement();
            if (element == null) continue;
            BuildSequence(element, set);
        }
    }


    /// <summary>
    /// Builds a linear sequence of outputs that feed into this output.
    /// </summary>
    /// <param name="outputElement"></param>
    /// <param name="set"></param>
    internal static void BuildSequence(OutputElement outputElement, in OrderedPushSet<OutputElement> set)
    {
        set.Add(outputElement);
        foreach (var output in outputElement.OwnerNode.AllInputElements())
        {
            var element = output.SourceElement();
            if (element is not null)
            {
                BuildSequence(element, set);
            }
        }
    }

    public static Dictionary<OperationElement, OrderedPushSet<OutputElement>> BuildFlowTable(NodeGroup group)
    {
        var usedOperations = new HashSet<OperationElement>();

        group.ForeachNode<INode>((n, ctx) =>
        {
            foreach (var impulseElement in n.AllImpulseElements())
            {
                var target = impulseElement.TargetElement();
                if (target == null) continue;
                usedOperations.Add(target);
            }
        }, true);

        var operationMap = new Dictionary<OperationElement, OrderedPushSet<OutputElement>>();

        foreach (var used in usedOperations)
        {
            var seq = new OrderedPushSet<OutputElement>();
            BuildSequence(used.OwnerNode, seq);
            operationMap[used] = seq;
        }

        return operationMap;
    }


    public static string TextRepresentation(NodeGroup group)
    {
        var builder = new StringBuilder();
        var table = BuildFlowTable(group);

        foreach (var (opIndex, (op, seq)) in table.Index())
        {
            builder.AppendLine($"@{opIndex} : {op.OwnerNode.GetType().GetNiceName()} {op.DisplayName}");
            builder.AppendLine("{");
            foreach (var (i, output) in seq.Index())
            {
                AppendSequenceItem(builder, seq, $"{i}", output.OwnerNode, output.DisplayName);
            }
            AppendSequenceItem(builder, seq, "%", op.OwnerNode, op.DisplayName);
            foreach (var impulse in op.OwnerNode.AllImpulseElements())
            {
                var target = impulse.TargetElement();
                if (target is null) continue;
                var id = table.Keys.Index().Where(n => n.Item == target).First().Index;
                builder.AppendLine($"|{impulse.DisplayName}: jump @{id}");
            }
            builder.AppendLine("}");
        }

        return builder.ToString();
    }

    private static void AppendSequenceItem(StringBuilder builder, OrderedPushSet<OutputElement> seq, string varName, INode node, string outputName)
    {
        builder.Append($"%{varName} = ({node.GetType().GetNiceName()}.{outputName}");
        foreach (var input in node.AllInputElements())
        {
            var inputSource = input.SourceElement();
            if (inputSource != null)
            {
                builder.Append($" {input.DisplayName}=%{seq.IndexOf(inputSource)}");
            }
            else
            {
                builder.Append($" {input.DisplayName}=<default>");
            }
        }
        builder.AppendLine($")");
    }
}