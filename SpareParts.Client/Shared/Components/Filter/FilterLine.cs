namespace SpareParts.Client.Shared.Components.Filter
{
    public class FilterLine
    {
        public FilterLine() : this(new FilterField("", typeof(string), true), "eq", "")
        {
            
        }

        public FilterLine(FilterField selectedField, string selectedOperator, string value)
        {
            SelectedField = selectedField;
            SelectedOperator = selectedOperator;
            Value = value;
        }

        public FilterField SelectedField { get; set; }
        public string SelectedOperator { get; set; }
        public string Value { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is FilterLine line &&
                   EqualityComparer<FilterField>.Default.Equals(SelectedField, line.SelectedField) &&
                   SelectedOperator == line.SelectedOperator &&
                   Value == line.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SelectedField, SelectedOperator, Value);
        }
    }
}
