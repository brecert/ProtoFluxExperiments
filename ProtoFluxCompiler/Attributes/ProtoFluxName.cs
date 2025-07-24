namespace ProtoFluxCompiler.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ProtoFluxNameAttribute(string name) : Attribute
{
    public readonly string Name = name;
}
