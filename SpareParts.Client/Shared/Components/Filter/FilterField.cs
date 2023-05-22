namespace SpareParts.Client.Shared.Components.Filter
{
    public class FilterField
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

        public override bool Equals(object? obj)
        {
            return obj is FilterField field &&
                   Name == field.Name &&
                   EqualityComparer<Type>.Default.Equals(Type, field.Type) &&
                   IsSelected == field.IsSelected;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type, IsSelected);
        }
    }
}
