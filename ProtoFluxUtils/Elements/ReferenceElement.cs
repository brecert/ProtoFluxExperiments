using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public readonly struct ReferenceElement(INode node, int index, int? elementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = node;
  public readonly int ElementIndex = index;
  public readonly int? ElementListIndex = elementListIndex;

  public readonly INode? Target
  {
    get => OwnerNode.GetReferenceTarget(ElementIndex);
    set => OwnerNode.SetReferenceTarget(ElementIndex, value);
  }

  public readonly string DisplayName =>
    ElementListIndex is int listIndex
      ? throw new NotImplementedException()
      : OwnerNode.GetReferenceName(ElementIndex);

  public readonly Type TargetType => OwnerNode.GetReferenceType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString()
  {
    return $"ReferenceElement.{TargetType} [{ElementIndex}] '{DisplayName}' -> {Target}";
  }
}
