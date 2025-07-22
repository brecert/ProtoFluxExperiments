namespace ProtoFluxCompiler.Core;

public sealed partial class Ref<T>(T value)
{
    public T Value = value;
    public static implicit operator T(Ref<T> @ref) => @ref;
}