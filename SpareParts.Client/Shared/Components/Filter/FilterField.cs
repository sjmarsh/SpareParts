namespace SpareParts.Client.Shared.Components.Filter
{
    public record FilterField
    {
        public FilterField(string name, Type type, bool isSelected)
        {
            Name = name;
            Type = type;
            IsSelected = isSelected;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsSelected { get; set; }
    }
}
