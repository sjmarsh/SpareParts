namespace SpareParts.Shared.Models
{
    public class PartGraphQLObject
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [UseFiltering]
        public List<PartAttribute>? Attributes { get; set; }
    }
}
