using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public record OperationElement(INode Node, int ElementIndex, int? ElementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = Node;

  public readonly int ElementIndex = ElementIndex;

  public readonly int? ElementListIndex = ElementListIndex;

  public IOperation? Target
  {
    get => GetOperation();
  }

  internal IOperation? GetOperation() =>
      ElementListIndex is int listIndex
        ? OwnerNode.GetOperationList(listIndex).GetOperation(ElementIndex)
        : OwnerNode.GetOperation(ElementIndex);

  public string DisplayName =>
    ElementListIndex is int listIndex
      ? $"{OwnerNode.GetOperationName(listIndex)}[{ElementIndex}]"
      : OwnerNode.GetOperationName(ElementIndex);


  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString() =>
    $"OperationElement [{ElementIndex}, {ElementListIndex}] '{DisplayName}' -> {Target}";
}
