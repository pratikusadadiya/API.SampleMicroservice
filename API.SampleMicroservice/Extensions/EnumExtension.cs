using System.ComponentModel;
using System.Reflection;

namespace API.SampleMicroservice.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<TEntity>(this TEntity source)
        {
            FieldInfo? fi = source?.GetType().GetField(source.ToString() ?? string.Empty);
            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                return source?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        public static bool ValidateEnumDescription<TEnum>(string fieldName) where TEnum : struct, Enum
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            return enumValues.Any(enumValue =>
                enumValue.GetDescription().Equals(fieldName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
