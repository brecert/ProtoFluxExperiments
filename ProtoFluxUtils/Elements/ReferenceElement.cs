using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public record ReferenceElement(INode Node, int ElementIndex, int? ElementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = Node;
  public readonly int ElementIndex = ElementIndex;
  public readonly int? ElementListIndex = ElementListIndex;

  public INode? Target
  {
    get => OwnerNode.GetReferenceTarget(ElementIndex);
    set => OwnerNode.SetReferenceTarget(ElementIndex, value);
  }

  public string DisplayName =>
    ElementListIndex is int listIndex
      ? throw new NotImplementedException()
      : OwnerNode.GetReferenceName(ElementIndex);

  public Type TargetType => OwnerNode.GetReferenceType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString()
  {
    return $"ReferenceElement.{TargetType} [{ElementIndex}] '{DisplayName}' -> {Target}";
  }
}
