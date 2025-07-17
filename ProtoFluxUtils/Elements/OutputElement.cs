using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public record OutputElement(INode Node, int ElementIndex, int? ElementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = Node;

  public readonly int ElementIndex = ElementIndex;

  public readonly int? ElementListIndex = ElementListIndex;

  public IOutput? Target
  {
    get => OwnerNode.GetOutput(ElementIndex);
  }

  internal IOutput? GetOutput() =>
    ElementListIndex is int listIndex
      ? OwnerNode.GetOutputList(listIndex).GetOutput(ElementIndex)
      : OwnerNode.GetOutput(ElementIndex);

  public string DisplayName =>
    ElementListIndex is int listIndex
      ? $"{OwnerNode.GetOutputListName(listIndex)}[{ElementIndex}]"
      : OwnerNode.GetOutputName(ElementIndex);

  public DataClass DataClass => OwnerNode.GetOutputTypeClass(ElementIndex);

  public Type ValueType => OwnerNode.GetOutputType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString() =>
    $"OutputElement.{DataClass} [{ElementIndex}, {ElementListIndex}] '{DisplayName}'";
}
