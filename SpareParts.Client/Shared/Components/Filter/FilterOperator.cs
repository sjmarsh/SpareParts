namespace SpareParts.Client.Shared.Components.Filter
{
    public static class FilterOperator
    {
        public const string Equal = "eq";
        public const string NotEqual = "neq";
        public const string GreaterThan = "gt";
        public const string GreatherThanOrEqual = "gte";
        public const string LessThan = "lt";
        public const string LessThanOrEqual = "lte";
        public const string StartsWith = "startsWith";
        public const string EndsWith = "endsWith";

        public static IEnumerable<string> Operators()
        {
            var flds = typeof(FilterOperator).GetFields().Where(fi => fi.IsLiteral && !fi.IsInitOnly);
            foreach(var fld in  flds)
            {
                if (fld.GetRawConstantValue() is string constValue)
                {
                    yield return constValue;
                }
            }
        }
    }
}
