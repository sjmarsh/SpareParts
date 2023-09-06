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
    }
}
