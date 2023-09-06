using SpareParts.Shared.Constants;
using System.Reflection;

namespace SpareParts.Shared.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetStringValueOrDefault<T>(this PropertyInfo prop, T value, string defaultValue = "", string formatTemplate = "", IFormatProvider? formatProvider = null)
        {
            if (prop is null || value is null)
            {
                return defaultValue;
            }

            var propValue = prop.GetValue(value);
            return GetStringResult(propValue, defaultValue, formatTemplate, formatProvider);
        }

        
        public static string GetStringValueWithDefaultFormat<T>(this PropertyInfo prop, T value, string defaultValue = "")
        {
            if (prop is null || value is null)
            {
                return defaultValue;
            }

            var propValue = prop.GetValue(value);
            var formatTemplate = DefaultStringFormat.GetFormatFor(prop.PropertyType);
            return GetStringResult(propValue, defaultValue, formatTemplate, null);
        }

        public static string GetStringValueWithDefaultFormat<T>(this T value, string defaultValue = "")
        {
            if(value is null)
            {
                return defaultValue;
            }
            var formatTemplate = DefaultStringFormat.GetFormatFor(typeof(T));
            return GetStringResult(value, defaultValue, formatTemplate, null);
        }

        private static string GetStringResult(object? propValue, string defaultValue, string formatTemplate, IFormatProvider? formatProvider)
        {
            var result = defaultValue;
            if (string.IsNullOrEmpty(formatTemplate))
            {
                result = (propValue ?? defaultValue).ToString() ?? defaultValue;
            }

            if (propValue is IFormattable formattable)
            {
                result = formattable.ToString(formatTemplate, formatProvider);
            }
            return result;
        }

    }
}
