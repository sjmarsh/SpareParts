using System.ComponentModel;

namespace SpareParts.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetEnumDescription(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var memInfo = type.GetMember(enumValue.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? ((DescriptionAttribute)attributes[0]).Description : null;
        }

        public static T? GetEnumFromString<T>(this string? value) where T : Enum
        {
            if(value is null)
            {
                return default;
            }

            if (Enum.TryParse(typeof(T), value, out var result))
            {
                return (T)result;
            }
            return default;
        }
    }
}
