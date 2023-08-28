namespace SpareParts.API.Entities
{
    public class PartAttribute : EntityBase
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Value { get; set; }
    }
}
