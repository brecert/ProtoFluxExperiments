using System.Linq.Expressions;
using System.Reflection;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution.Nodes.Operators;
using ProtoFluxCompiler.Attributes;
using ProtoFluxUtils.Elements;
using ProtoFluxUtils.Extensions;

namespace ProtoFluxCompiler.Compiler;

public class NodeGroupCompiler
{
    record NodeInstance(ParameterExpression Variable, Expression Expression);

    readonly NodeGroup nodeGroup;
    readonly INode[] nodes;

    readonly Dictionary<INode, NodeInstance> instanceMap = [];
    readonly Dictionary<OperationElement, ParameterExpression> operationMap = [];

    NodeGroupCompiler(NodeGroup nodeGroup)
    {
        var nodes = new INode[nodeGroup.TotalNodeCount];
        var i = 0;
        nodeGroup.ForeachNode<INode>((a, _) => nodes[i++] = a, true);

        this.nodes = nodes;
        this.nodeGroup = nodeGroup;
    }

    public static Action Compile(NodeGroup nodeGroup) =>
        (Action)new NodeGroupCompiler(nodeGroup).Compile().Compile();

    LambdaExpression Compile()
    {
        var instances = CreateInstances();
        var references = AssignReferences();
        var blocks = CreateBlocks();
        var impulses = AssignImpulses();

        var combined = Expression.Block([
            ..instances,
            // ..references,
            // ..blocks,
            // ..impulses
        ]);

        return Expression.Lambda(combined);
    }


    IEnumerable<Expression> CreateInstances()
    {
        foreach (var (i, node) in nodes.Index())
        {
            var newNodeType = NodeRemapper.RemapType(node.GetType());
            var variable = Expression.Variable(newNodeType, $"n{i} {node.GetType().Name}");
            var expression = Expression.Assign(
                variable,
                Expression.New(newNodeType)
            );
            instanceMap.Add(node, new(variable, expression));
            yield return expression;
        }
    }

    IEnumerable<Expression> AssignReferences()
    {
        foreach (var node in nodes)
        {
            var (variable, _) = instanceMap[node];
            // TODO: get references by attribute
            foreach (var reference in node.AllReferenceElements())
            {
                if (reference.Target == null) continue;
                var targetReference = instanceMap[reference.Target].Variable;
                var member = variable.Type.GetMember(reference.DisplayName)[0];
                var memberAccess = Expression.MakeMemberAccess(variable, member);
                yield return Expression.Assign(memberAccess, targetReference);
            }
        }
    }

    IEnumerable<Expression> CreateBlocks()
    {
        var table = Reflow.BuildFlowTable(nodeGroup);

        foreach (var (operation, sequence) in table)
        {
            var variable = Expression.Variable(typeof(Action), operation.DisplayName);
            operationMap[operation] = variable;

            yield return Expression.Assign(
                variable,
                Expression.Lambda<Action>(Expression.Block(BuildSequence(sequence)))
            );
        }
    }

    private IEnumerable<Expression> BuildSequence(Collections.OrderedPushSet<OutputElement> sequence)
    {
        var outputMap = new Dictionary<OutputElement, Expression>();

        foreach (var output in sequence)
        {
            if (output.Target == null)
            {
                throw new Exception("Unable to compile sequence, output has null target");
            }

            // TODO: get outputs by attribute
            var (owner, _) = instanceMap[output.OwnerNode];
            var method = GetMethodByName(owner.Type, output.DisplayName);

            // TODO: use attributes/metadata for Default
            var inputs = output.OwnerNode.AllInputElements()
                .Select(i => i.SourceElement() is var el and not null ? outputMap[el] : Expression.Default(i.ValueType));

            var variable = Expression.Variable(method.ReturnType, method.Name);
            outputMap[output] = variable;

            yield return Expression.Assign(
                variable,
                Expression.Call(owner, method, inputs)
            );
        }
    }

    IEnumerable<Expression> AssignImpulses()
    {
        foreach (var (node, (mappedNodeVariable, _)) in instanceMap)
        {
            foreach (var impulseElement in node.AllImpulseElements())
            {
                var targetOperation = impulseElement.TargetElement();
                if (targetOperation == null) continue;


                var impulseMember = GetMemberByName(mappedNodeVariable.Type, impulseElement.DisplayName);

                yield return Expression.Assign(
                    Expression.MakeMemberAccess(mappedNodeVariable, impulseMember),
                    operationMap[targetOperation]
                );
            }
        }
    }

    static MethodInfo GetMethodByName(Type type, string name) =>
        type.GetMethods()
            .FirstOrDefault(m => (m.GetCustomAttribute<ProtoFluxNameAttribute>()?.Name ?? m.Name) == name)
            ?? throw new Exception($"Unable to find method '{name}' by name for '{type}'");

    static MemberInfo GetMemberByName(Type type, string name) =>
        type.GetMembers()
            .FirstOrDefault(m => (m.GetCustomAttribute<ProtoFluxNameAttribute>()?.Name ?? m.Name) == name)
            ?? throw new Exception($"Unable to find method '{name}' by name for '{type}'");

}