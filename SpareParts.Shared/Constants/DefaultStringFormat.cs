namespace SpareParts.Shared.Constants
{
    public class DefaultStringFormat
    {
        public const string ForDate = "dd/MM/yyyy";
        public const string ForDecimal = "F2";
        public const string ForCurrency = "C2";

        private static readonly Dictionary<Type, string> _stringFormatLookup = new()
        {
            { typeof(DateTime), ForDate },
            { typeof(DateTime?), ForDate },
            { typeof(double), ForDecimal },
            { typeof(double?), ForDecimal }
        };

        public static string GetFormatFor(Type type, string defaultValue = "")
        {
            return _stringFormatLookup.GetValueOrDefault(type, defaultValue);
        }
    }
}
