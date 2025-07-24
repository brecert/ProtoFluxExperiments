using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public record InputElement(INode OwnerNode, int ElementIndex, int? ElementListIndex = null) : IElementIndex
{
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
        ? OwnerNode.GetInputList(listIndex).GetInputSource(ElementIndex)
        : OwnerNode.GetInputSource(ElementIndex);

  internal void SetInputSource(IOutput? value)
  {
    if (ElementListIndex is int listIndex)
    {
      OwnerNode.GetInputList(listIndex).SetInputSource(ElementIndex, value);
    }
    else
    {
      OwnerNode.SetInputSource(ElementIndex, value);
    }
  }

  public string DisplayName =>
    ElementListIndex is int listIndex
      ? $"{OwnerNode.GetInputListName(listIndex)}[{ElementIndex}]"
      : OwnerNode.GetInputName(ElementIndex);

  public Type ValueType => OwnerNode.GetInputType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;

  public override string ToString() =>
    $"InputElement.{ValueType} [{ElementIndex}, {ElementListIndex}] '{DisplayName}' <- {Source}";
}
