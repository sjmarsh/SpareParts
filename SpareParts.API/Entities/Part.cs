namespace SpareParts.API.Entities
{
    public class Part
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public double Weight { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<InventoryItem>? InventoryItems { get; set; }
    }
}
