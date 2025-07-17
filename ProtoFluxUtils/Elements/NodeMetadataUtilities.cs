using System.Reflection;
using System.Runtime.CompilerServices;
using Elements.Core;
using FrooxEngine.ProtoFlux;
using HarmonyLib;
using ProtoFlux.Core;
using DotNetUtils.Extensions;

namespace ProtoFluxUtils.Elements;

static class NodeMetadataUtilities
{
  // internal static IEnumerable<MenuItem> MenuItems(ProtoFluxTool __instance, IWorldElement? grabbedReference)
  // {
  //   if (grabbedReference == null) yield break;

  //   foreach (var nodeType in NodeTypes())
  //   {
  //     var globalRefMeta = GlobalRefMetadata(nodeType).Where(m => !m.ValueType.IsGenericTypeDefinition).FirstOrDefault();
  //     if (globalRefMeta != null && globalRefMeta.ValueType.IsAssignableFrom(grabbedReference.GetType()))
  //     {
  //       yield return new MenuItem(nodeType);
  //     }
  //   }
  // }

  public readonly struct InputElementMetadata(int index, FieldInfo field, Type inputType)
  {
    public readonly int Index = index;
    public readonly FieldInfo Field = field;
    public readonly Type InputType = inputType;
  }

  public static IEnumerable<InputElementMetadata> InputMetadata(Type type)
  {
    var index = 0;
    foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
    {
      if (field.FieldType.TryGetGenericTypeDefinition(out var genericTypeDefinition) && typeof(ValueInput<>) == genericTypeDefinition || typeof(ValueArgument<>) == genericTypeDefinition || typeof(ObjectInput<>) == genericTypeDefinition || typeof(ObjectArgument<>) == genericTypeDefinition)
      {
        yield return new(index++, field, field.FieldType.GenericTypeArguments[0]);
      }
    }
  }


  // lighter than GetMetadata
  internal static IEnumerable<GlobalRefMetadata> GlobalRefMetadata(Type type)
  {
    var index = 0;
    foreach (var field in type.EnumerateAllInstanceFields(BindingFlags.Instance | BindingFlags.Public))
    {
      if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(GlobalRef<>))
      {
        yield return new GlobalRefMetadata(index++, field);
      }
    }
  }

  public static IEnumerable<Type> NodeTypes() =>
    Traverse.Create(typeof(ProtoFluxHelper)).Field<Dictionary<Type, Type>>("protoFluxToBindingMapping").Value.Keys;

  // [HarmonyReversePatch]
  // [HarmonyPatch(typeof(ProtoFluxHelper), "GetNodeForType")]
  // [MethodImpl(MethodImplOptions.NoInlining)]
  [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "GetNodeForType")]
  extern static Type GetNodeForType(Type type, List<NodeTypeRecord> list);
  // internal static Type GetNodeForType(Type type, List<NodeTypeRecord> list) => throw new NotImplementedException();

}