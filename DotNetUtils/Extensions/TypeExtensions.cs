using System.Diagnostics.CodeAnalysis;

namespace DotNetUtils.Extensions;

public static class TypeExtensions
{
    public static bool TryGetGenericTypeDefinition(this Type type, [NotNullWhen(true)] out Type? genericTypeDefinition)
    {
        if (type.IsGenericType)
        {
            genericTypeDefinition = type.GetGenericTypeDefinition();
            return true;
        }
        genericTypeDefinition = null;
        return false;
    }
}