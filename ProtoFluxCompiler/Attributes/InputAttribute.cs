namespace ProtoFluxCompiler.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class InputAttribute(string? @default = null) : Attribute
{
    public readonly string? DefaultMethodName = @default;
}
