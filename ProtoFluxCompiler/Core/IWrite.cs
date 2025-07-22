namespace ProtoFluxCompiler.Core;

public interface IWrite<T>
{
    bool Write(T value);
}
