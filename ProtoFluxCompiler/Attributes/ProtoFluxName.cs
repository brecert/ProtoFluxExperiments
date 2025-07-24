namespace ProtoFluxCompiler.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class ProtoFluxNameAttribute(string name) : Attribute
{
    public readonly string Name = name;
}
