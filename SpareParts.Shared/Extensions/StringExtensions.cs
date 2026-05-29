namespace SpareParts.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool TryConvertXmlToTimeSpan(this string target, out TimeSpan result)
        {
            result = TimeSpan.Zero;
            if (string.IsNullOrEmpty(target))
            {
                return false;
            }
            try
            {
                result = System.Xml.XmlConvert.ToTimeSpan(target);
                return true;
            }
            catch (FormatException)
            {
                result = TimeSpan.Zero;
                return false;
            }
        }
    }
}
