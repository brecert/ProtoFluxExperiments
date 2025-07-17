using System.Collections;

namespace ProtoFluxCompiler.Collections;

// TODO: use an optimized data structure for this
public class OrderedPushSet<T> : ICollection<T>
{
    readonly List<T> list = [];

    public int Count => list.Count;

    public bool IsReadOnly => ((ICollection<T>)list).IsReadOnly;

    public void Add(T item)
    {
        list.Remove(item);
        list.Insert(0, item);
    }

    public int IndexOf(T item) => list.IndexOf(item);

    public void Clear() => list.Clear();

    public bool Contains(T item) => list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

    public bool Remove(T item) => list.Remove(item);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
