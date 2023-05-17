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

        public override bool Equals(object? obj)
        {
            return obj is FilterField field &&
                   Name == field.Name &&
                   EqualityComparer<Type>.Default.Equals(Type, field.Type);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type);
        }
    }
}
