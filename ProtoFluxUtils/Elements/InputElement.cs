using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public record InputElement(INode Node, int ElementIndex, int? ElementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = Node;

  public readonly int ElementIndex = ElementIndex;

  public readonly int? ElementListIndex = ElementListIndex;


  public IOutput? Source
    {
        get => GetInputSource();
        set => SetInputSource(value);
    }

  public OutputElement? SourceElement()
  {
    if (Source == null) return null;
    Source.FindOutputIndex(out var index, out var listIndex);
    if (listIndex >= 0)
    {
      return new(Source.OwnerNode, index, listIndex);
    }
    else
    {
      return new(Source.OwnerNode, index, null);
    }
  }

  internal IOutput? GetInputSource() =>
      ElementListIndex is int listIndex
        ? Node.GetInputList(listIndex).GetInputSource(ElementIndex)
        : Node.GetInputSource(ElementIndex);

  internal void SetInputSource(IOutput? value)
  {
    if (ElementListIndex is int listIndex)
    {
      Node.GetInputList(listIndex).SetInputSource(ElementIndex, value);
    }
    else
    {
      Node.SetInputSource(ElementIndex, value);
    }
  }

  public string DisplayName =>
    ElementListIndex is int listIndex
      ? $"{Node.GetInputListName(listIndex)}[{ElementIndex}]"
      : Node.GetInputName(ElementIndex);

  public Type ValueType => Node.GetInputType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString() =>
    $"InputElement.{ValueType} [{ElementIndex}, {ElementListIndex}] '{DisplayName}' <- {Source}";
}
