namespace SpareParts.Shared.Models
{
    public class InventoryItemDetail : InventoryItem
    {
        // required for serialization
        public InventoryItemDetail()
        {
        }

        public InventoryItemDetail(int id, int partId, string? partName, int quantity, DateTime dateRecorded)
        {
            ID = id;
            PartID = partId;
            PartName = partName;
            Quantity = quantity;
            DateRecorded = dateRecorded;
        }

        public string? PartName { get; set; }
    }

    public class InventoryItemDetailResponse : ResponseBase<InventoryItemDetail>
    {
    }

    public class InventoryItemDetailListResponse : ResponseListBase<InventoryItemDetail>
    {
    }
}
