namespace SpareParts.Shared.Models
{
    public class Part : ModelBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<PartAttribute>? Attributes { get; set; }
    }

    public class PartResponse : ResponseBase<Part>
    {
    }

    public class PartListResponse : ResponseListBase<Part>
    {
    }
}
