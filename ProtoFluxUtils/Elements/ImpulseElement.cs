using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public record ImpulseElement(INode Node, int ElementIndex, int? ElementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = Node;

  public readonly int ElementIndex = ElementIndex;

  public readonly int? ElementListIndex = ElementListIndex;

  public IOperation? Target
  {
    get => GetImpulseTarget();
    set => SetImpulseTarget(value);
  }

  public OperationElement? TargetElement()
  {
    if (Target == null) return null;
    Target.FindOperationIndex(out var index, out var listIndex);
    if (listIndex >= 0)
    {
      return new(Target.OwnerNode, index, listIndex);
    }
    else
    {
      return new(Target.OwnerNode, index, null);
    }
  }

  internal IOperation? GetImpulseTarget() =>
      ElementListIndex is int listIndex
        ? OwnerNode.GetImpulseList(listIndex).GetImpulseTarget(ElementIndex)
        : OwnerNode.GetImpulseTarget(ElementIndex);

  internal void SetImpulseTarget(IOperation? value)
  {
    if (ElementListIndex is int listIndex)
    {
      OwnerNode.GetImpulseList(listIndex).SetImpulseTarget(ElementIndex, value);
    }
    else
    {
      OwnerNode.SetImpulseTarget(ElementIndex, value);
    }
  }

  public string DisplayName =>
    ElementListIndex is int listIndex
      ? $"{OwnerNode.GetImpulseListName(listIndex)}[{ElementIndex}]"
      : OwnerNode.GetImpulseName(ElementIndex);

  public ImpulseType TargetType => OwnerNode.GetImpulseType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString() =>
    $"ImpulseElement.{TargetType} [{ElementIndex}, {ElementListIndex}] '{DisplayName}' -> {Target}";
}
