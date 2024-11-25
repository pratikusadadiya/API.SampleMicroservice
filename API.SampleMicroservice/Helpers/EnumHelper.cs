using FluentValidation;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi != null ?
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false) : [];

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetDescriptionFromValue(Type enumType, int value)
        {
            foreach (var field in enumType.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if ((int)field?.GetValue(null) == value)
                    {
                        return attribute.Description;
                    }
                }
            }
            return string.Empty; // Or some default value
        }

        public static int GetValueFromNameOrDefault<T>(string name) where T : Enum
        {
            if (string.IsNullOrEmpty(name))
            {
                return 0;
            }

            foreach (var field in typeof(T).GetFields())
            {
                if (field.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return (int)field.GetValue(null);
                }
            }

            return 0;
        }

        public static IRuleBuilderOptions<T, string> IsInEnum<T, TEnum>(this IRuleBuilder<T, string> ruleBuilder) where TEnum : struct, Enum
        {
            var allowedValues = Enum.GetValues(typeof(TEnum))
                           .Cast<TEnum>()
                           .Where(e => GetDescription(e) != null)
                           .Select(e => GetDescription(e)!)
                           .ToHashSet(StringComparer.Ordinal);

            return ruleBuilder.Must(value =>
            {
                if (string.IsNullOrEmpty(value))
                    return false;

                return allowedValues.Contains(value);
            });
        }
    }
}