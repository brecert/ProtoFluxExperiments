using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using DotNext.Linq.Expressions;
using DotNext.Threading;
using ExpressionToCodeLib;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution.Nodes.Operators;
using ProtoFluxCompiler.Attributes;
using ProtoFluxUtils.Elements;
using ProtoFluxUtils.Extensions;
using static DotNext.Metaprogramming.CodeGenerator;

namespace ProtoFluxCompiler.Compiler;

public class NodeGroupCompiler
{
    record NodeInstance(ParameterExpression Variable, Expression Expression);

    readonly NodeGroup nodeGroup;
    readonly INode[] nodes;

    readonly Dictionary<INode, ParameterExpression> instanceMap = [];
    readonly Dictionary<OperationElement, ParameterExpression> operationMap = [];

    static T Debug<T>(T any) where T : Expression
    {
        System.Diagnostics.Debug.WriteLine(ExpressionToCode.ToCode(any));
        return any;
    }

    NodeGroupCompiler(NodeGroup nodeGroup)
    {
        var nodes = new INode[nodeGroup.TotalNodeCount];
        var i = 0;
        nodeGroup.ForeachNode<INode>((a, _) => nodes[i++] = a, true);

        this.nodes = nodes;
        this.nodeGroup = nodeGroup;
    }

    public static Action<Action<Core.INode, Core.INode>> Compile(NodeGroup nodeGroup) =>
        Debug(new NodeGroupCompiler(nodeGroup).Compile()).Compile();

    Expression<Action<Action<Core.INode, Core.INode>>> Compile()
    {
        // var instances = CreateInstances();
        // var references = AssignReferences();
        // var blocks = CreateBlocks();
        // var impulses = AssignImpulses();

        // // var combined = Expression.Block([
        // //     ..instances,
        // //     // ..references,
        // //     // ..blocks,
        // //     // ..impulses
        // // ]);

        // return Expression.Lambda(combined);

        return Lambda<Action<Action<Core.INode, Core.INode>>>(fun =>
        {
            CreateInstances();
            AssignReferences();
            CreateBlocks();
            AssignImpulses();

            Invoke(fun[0], [instanceMap.First().Value, instanceMap.Last().Value]);
        });
    }


    void CreateInstances()
    {
        foreach (var (i, node) in nodes.Index())
        {
            var newNodeType = NodeRemapper.RemapType(node.GetType());
            var variable = DeclareVariable(newNodeType, $"n{i}");
            Assign(
                variable,
                newNodeType.New()
            );
            instanceMap.Add(node, variable);

            foreach (var intoField in newNodeType.GetFields())
            {
                if (intoField.GetCustomAttribute<ConstantAttribute>() is null) continue;
                var name = ProtoFluxName(intoField);
                var fromField = node.GetType().GetField(name)
                    ?? throw new Exception($"Invalid ProtoFlux constant field mapping for '{name}'");
                Assign(
                    Expression.MakeMemberAccess(variable, intoField),
                    Expression.Constant(fromField.GetValue(node))
                );
                // WriteLine(Expression.MakeMemberAccess(variable, intoField));
                // WriteLine(Expression.Constant(fromField.GetValue(node)));
            }
        }
    }

    void AssignReferences()
    {
        foreach (var node in nodes)
        {
            var variable = instanceMap[node];
            // TODO: get references by attribute
            foreach (var reference in node.AllReferenceElements())
            {
                if (reference.Target == null) continue;
                var targetReference = instanceMap[reference.Target];
                var member = variable.Type.GetMember(reference.DisplayName)[0];
                var memberAccess = Expression.MakeMemberAccess(variable, member);
                Assign(memberAccess, targetReference);
            }
        }
    }

    void CreateBlocks()
    {
        var table = Reflow.BuildFlowTable(nodeGroup);

        foreach (var (i, (operation, sequence)) in table.Index())
        {
            var outputMap = new Dictionary<OutputElement, Expression>();

            var variable = DeclareVariable<Action>($"b{i}");
            operationMap[operation] = variable;

            Assign(
                variable,
                Lambda<Action>(fun =>
                {
                    BuildSequence(sequence, outputMap);
                    
                    // Finally we "jump" to the next operation by calling the operation
                    var node = instanceMap[operation.OwnerNode];
                    var operationMember = GetOperationByName(node.Type, operation.DisplayName);
                    var parameters = MapInputParameters(operation, outputMap, node, operationMember);
                    Call(node, operationMember, parameters);
                })
            );
        }

        static IEnumerable<Expression> MapInputParameters(OperationElement operation, Dictionary<OutputElement, Expression> outputMap, ParameterExpression node, MethodInfo operationMember)
        {
            foreach (var parameterInfo in operationMember.GetParameters())
            {
                var name = ProtoFluxName(parameterInfo) ?? throw new Exception($"No input name found for input on {node}");
                var inputElement = operation.OwnerNode.GetInputElementByName(name);
                if (inputElement?.SourceElement() is var sourceOutput and not null)
                {
                    var outputVarExpr = outputMap[sourceOutput];
                    yield return outputVarExpr;
                }
                else
                {
                    throw new Exception("default inputs are not supported yet");
                }
            }
        }
    }

    private void BuildSequence(Collections.OrderedPushSet<OutputElement> sequence, Dictionary<OutputElement, Expression> outputMap)
    {
        // var outputMap = new Dictionary<OutputElement, Expression>();

        foreach (var (i, output) in sequence.Index())
        {
            if (output.Target == null)
            {
                throw new Exception("Unable to compile sequence, output has null target");
            }

            // TODO: get outputs by attribute
            var owner = instanceMap[output.OwnerNode];
            var method = GetMethodByName(owner.Type, output.DisplayName);

            // TODO: use attributes/metadata for Default
            var inputs = output.OwnerNode.AllInputElements()
                .Select(i => i.SourceElement() is var el and not null ? outputMap[el] : Expression.Default(i.ValueType));

            var variable = DeclareVariable(method.ReturnType, $"o{i}");
            outputMap[output] = variable;

            Assign(
                variable,
                Expression.Call(owner, method, inputs)
            );
        }
    }

    void AssignImpulses()
    {
        foreach (var (node, mappedNodeVariable) in instanceMap)
        {
            foreach (var impulseElement in node.AllImpulseElements())
            {
                var targetOperation = impulseElement.TargetElement();
                if (targetOperation == null) continue;


                var impulseMember = GetMemberByName(mappedNodeVariable.Type, impulseElement.DisplayName);

                Assign(
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

    static MethodInfo GetOperationByName(Type type, string name) =>
        type.GetMethods()
            .Where(m => m.GetCustomAttribute<OperationAttribute>() != null)
            .FirstOrDefault(m => ProtoFluxName(m) == name)
            ?? throw new Exception($"Unable to find operation '{name}' by name for '{type}'");


    static string ProtoFluxName(MemberInfo info) =>
        info.GetCustomAttribute<ProtoFluxNameAttribute>()?.Name ?? info.Name;

    static string? ProtoFluxName(ParameterInfo info) =>
        info.GetCustomAttribute<ProtoFluxNameAttribute>()?.Name ?? info.Name;

}