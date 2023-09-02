namespace SpareParts.Client.Shared.Components.Filter
{
    public record FilterField
    {
        public FilterField(string name, Type type, bool isSelected, string? parentFieldName = null)
        {
            Name = name;
            Type = type;
            IsSelected = isSelected;
            ParentFieldName = parentFieldName;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsSelected { get; set; }

        public string? ParentFieldName { get; set; }
    }
}
