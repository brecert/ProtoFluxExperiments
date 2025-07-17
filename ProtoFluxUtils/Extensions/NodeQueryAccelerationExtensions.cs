using ProtoFlux.Core;
using ProtoFluxUtils.Elements;

namespace ProtoFluxUtils.Extensions;

public static partial class NodeQueryAccelerationExtensions
{
  public static IEnumerable<ImpulseElement> GetImpulsingElements(this NodeQueryAcceleration query, INode node) =>
    query.GetImpulsingNodes(node).SelectMany(n => n.AllImpulseElements().Where(i => i.Target?.OwnerNode == node));

  public static IEnumerable<InputElement> GetEvaluatingElements(this NodeQueryAcceleration query, INode node) =>
    query.GetEvaluatingNodes(node).SelectMany(n => n.AllInputElements().Where(i => i.Source?.OwnerNode == node));

  public static IEnumerable<ReferenceElement> GetReferencingElements(this NodeQueryAcceleration query, INode node) =>
    query.GetReferencingNodes(node).SelectMany(n => n.AllReferenceElements().Where(r => r.Target == node));
}