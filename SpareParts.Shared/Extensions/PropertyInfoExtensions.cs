using System.Reflection;

namespace SpareParts.Shared.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetStringValueOrDefault<T>(this PropertyInfo prop, T value, string defaultValue = "")
        {
            if (prop is null || value is null)
            {
                return defaultValue;
            }

            return (prop.GetValue(value) ?? "").ToString() ?? ""; // TODO - this could be expanded on to format types
        }
    }
}
