namespace SpareParts.Shared.Models
{
    public class InventoryItem : ModelBase
    {
        public int PartID { get; set; }
        public int Quantity { get; set; }
        public DateTime DateRecorded { get; set; }
    }

    public class InventoryItemResponse : ResponseBase<InventoryItem>
    {
    }

    public class InventoryItemListResponse : ResponseListBase<InventoryItem>
    {
    }

    // Using this to work-around issue validating collections with Fluent
    public class StocktakeModel
    {
        public List<InventoryItemDetail>? InventoryItems { get; set; }
    }    
}
