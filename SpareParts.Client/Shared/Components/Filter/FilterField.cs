namespace SpareParts.Client.Shared.Components.Filter
{
    public class FilterField
    {
        public FilterField(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
