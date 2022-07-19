namespace SpareParts.API.Entities
{
    public class InventoryItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public DateTime DateRecorded { get; set; }
                        
        public int? PartID { get; set; }
    }
}
