using HotChocolate.Types.Descriptors;

namespace SpareParts.API.GraphQL
{
    public class SparePartsNamingConventions : DefaultNamingConventions
    {
        // ref: https://stackoverflow.com/questions/66807364/how-can-you-control-the-serialization-of-enum-values-in-hotchocolate
        public override string GetEnumValueName(object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var stringVal = value.ToString();
            if (stringVal is null)
            {
                throw new ArgumentException("String value is null");
            }

            return stringVal.ToUpperInvariant();
        }
    }
}
