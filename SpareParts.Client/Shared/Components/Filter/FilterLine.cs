namespace SpareParts.Client.Shared.Components.Filter
{
    public class FilterLine
    {
        public FilterLine()
        {
            SelectedField = "";
            SelectedOperator = "eq";
            Value = "";
        }

        public string SelectedField { get; set; }
        public string SelectedOperator { get; set; }
        public string Value { get; set; }
    }
}
