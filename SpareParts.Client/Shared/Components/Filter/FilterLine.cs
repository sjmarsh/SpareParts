namespace SpareParts.Client.Shared.Components.Filter
{
    public record FilterLine
    {
        public FilterLine() : this(new FilterField("", typeof(string), true), FilterOperator.Equal, "")
        {
        }

        public FilterLine(FilterField selectedField, string selectedOperator, string value)
        {
            ID = Guid.NewGuid();
            SelectedField = selectedField;
            SelectedOperator = selectedOperator;
            Value = value;
        }

        public Guid ID { get; set; }

        public FilterField SelectedField { get; set; }
        public string SelectedOperator { get; set; }
        public string Value { get; set; }
    }
}
