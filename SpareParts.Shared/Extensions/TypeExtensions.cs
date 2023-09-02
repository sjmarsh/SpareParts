namespace SpareParts.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsStringType(this Type type)
        {
            return type == typeof(string);
        }

        public static bool IsNumericType(this Type type)
        {
            var numericTypes = new[] { typeof(int), typeof(long), typeof(double), typeof(int?), typeof(long?), typeof(double?) };
            return numericTypes.Contains(type);
        }

        public static bool IsDateType(this Type type)
        {
            var dateTimeTypes = new[] { typeof(DateTime), typeof(DateTime?), typeof(TimeSpan), typeof(TimeSpan?) };
            return dateTimeTypes.Contains(type);
        }

        public static bool IsListType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
