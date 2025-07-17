using System.Collections;
using System.Diagnostics;
using System.Text;
using Elements.Core;
using HarmonyLib;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;
using ProtoFlux.Runtimes.Execution.Nodes.Operators;
using ProtoFluxCompiler.Collections;
using ProtoFluxUtils.Elements;
using ProtoFluxUtils.Extensions;
using ExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ProtoFluxCompiler.Compiler;

public static class Reflow
{
    public static void BuildSequence<C>(INode node, in OrderedPushSet<OutputElement> set) where C : ExecutionContext
    {
        foreach (var input in node.AllInputElements())
        {
            var element = input.SourceElement();
            if (element == null) continue;
            BuildSequence<C>(element, set);
        }
    }


    public static void BuildSequence<C>(OutputElement outputElement, in OrderedPushSet<OutputElement> set) where C : ExecutionContext
    {
        set.Add(outputElement);
        foreach (var output in outputElement.OwnerNode.AllInputElements())
        {
            var element = output.SourceElement();
            if (element is not null)
            {
                BuildSequence<C>(element, set);
            }
        }
    }

    public static Dictionary<OperationElement, OrderedPushSet<OutputElement>> BuildFlowTable<C>(NodeGroup group) where C : ExecutionContext
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
            BuildSequence<C>(used.OwnerNode, seq);
            operationMap[used] = seq;
        }

        return operationMap;
    }

    public static string TextRepresentation<C>(NodeGroup group) where C : ExecutionContext
    {
        var builder = new StringBuilder();
        var table = BuildFlowTable<C>(group);
        var query = new NodeQueryAcceleration(group);

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