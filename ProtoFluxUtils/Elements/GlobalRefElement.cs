using ProtoFlux.Core;

namespace ProtoFluxUtils.Elements;

public record GlobalRefElement(INode Node, int ElementIndex, int? ElementListIndex = null) : IElementIndex
{
  public readonly INode OwnerNode = Node;

  public readonly int ElementIndex = ElementIndex;

  public readonly int? ElementListIndex = ElementListIndex;

  public Global? Target
  {
    get => OwnerNode.GetGlobalRefBinding(ElementIndex);
    set => OwnerNode.SetGlobalRefBinding(ElementIndex, value);
  }

  public string DisplayName =>
    ElementListIndex is int listIndex
      ? throw new NotImplementedException()
      : OwnerNode.GetGlobalRefName(ElementIndex);


  public Type ValueType => OwnerNode.GetGlobalRefValueType(ElementIndex);

  int IElementIndex.ElementIndex => ElementIndex;

  int? IElementIndex.ElementListIndex => ElementListIndex;
}
