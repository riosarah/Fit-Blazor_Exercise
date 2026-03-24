using System.Reflection;

namespace Shared.Utilities;

public static class ObjectMemberComparer
{
    private static readonly BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

    public static bool MembersEqual(object? left, object? right) => MembersEqual(left, right, DefaultFlags);

    public static bool MembersEqual(object? left, object? right, BindingFlags bindingFlags)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        var leftType = left.GetType();
        var rightType = right.GetType();

        var propertyComparer = BuildPropertyMap(rightType, bindingFlags);

        foreach (var property in leftType.GetProperties(bindingFlags))
        {
            if (!property.CanRead || property.GetIndexParameters().Length > 0)
                continue;

            if (!propertyComparer.TryGetValue(property.Name, out var otherProperty))
                continue;

            if (property.PropertyType != otherProperty.PropertyType)
                continue;

            var leftValue = property.GetValue(left);
            var rightValue = otherProperty.GetValue(right);

            if (!Equals(leftValue, rightValue))
                return false;
        }

        var fieldComparer = BuildFieldMap(rightType, bindingFlags);

        foreach (var field in leftType.GetFields(bindingFlags))
        {
            if (!fieldComparer.TryGetValue(field.Name, out var otherField))
                continue;

            if (field.FieldType != otherField.FieldType)
                continue;

            var leftValue = field.GetValue(left);
            var rightValue = otherField.GetValue(right);

            if (!Equals(leftValue, rightValue))
                return false;
        }

        return true;
    }

    private static Dictionary<string, PropertyInfo> BuildPropertyMap(Type type, BindingFlags flags)
    {
        var map = new Dictionary<string, PropertyInfo>(StringComparer.Ordinal);
        foreach (var property in type.GetProperties(flags))
        {
            if (!property.CanRead || property.GetIndexParameters().Length > 0)
                continue;

            map[property.Name] = property;
        }

        return map;
    }

    private static Dictionary<string, FieldInfo> BuildFieldMap(Type type, BindingFlags flags)
    {
        var map = new Dictionary<string, FieldInfo>(StringComparer.Ordinal);
        foreach (var field in type.GetFields(flags))
        {
            map[field.Name] = field;
        }

        return map;
    }
}
