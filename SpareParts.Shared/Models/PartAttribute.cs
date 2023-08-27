namespace SpareParts.Shared.Models
{
    public class PartAttribute : ModelBase
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Value { get; set; }
    }
}
