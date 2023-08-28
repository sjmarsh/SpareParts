namespace SpareParts.API.Entities
{
    public class InventoryItem : EntityBase
    {
        public int Quantity { get; set; }
        public DateTime DateRecorded { get; set; }
                        
        public int? PartID { get; set; }
    }
}
