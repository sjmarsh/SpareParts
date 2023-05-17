namespace SpareParts.Client.Shared.Components.Filter
{
    public class FilterLine
    {
        public FilterLine()
        {
            SelectedField = new FilterField("", typeof(object));
            SelectedOperator = "eq";
            Value = "";
        }

        public FilterField SelectedField { get; set; }
        public string SelectedOperator { get; set; }
        public string Value { get; set; }
    }
}
