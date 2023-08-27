namespace SpareParts.API.Entities
{
    public class PartAttribute
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Value { get; set; }
    }
}
