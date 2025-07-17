using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public readonly struct OutputElement(INode node, int elementIndex, int? elementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = node;

  public readonly int ElementIndex = elementIndex;

  public readonly int? ElementListIndex = elementListIndex;

  public readonly IOutput? Target
  {
    get => OwnerNode.GetOutput(ElementIndex);
  }

  internal IOutput? GetOutput() =>
    ElementListIndex is int listIndex
      ? OwnerNode.GetOutputList(listIndex).GetOutput(ElementIndex)
      : OwnerNode.GetOutput(ElementIndex);

  public readonly string DisplayName =>
    ElementListIndex is int listIndex
      ? $"{OwnerNode.GetOutputListName(listIndex)}[{ElementIndex}]"
      : OwnerNode.GetOutputName(ElementIndex);

  public readonly DataClass DataClass => OwnerNode.GetOutputTypeClass(ElementIndex);

  public readonly Type ValueType => OwnerNode.GetOutputType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString() =>
    $"OutputElement.{DataClass} [{ElementIndex}, {ElementListIndex}] '{DisplayName}'";
}
