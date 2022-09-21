using Refit;
using SpareParts.Shared.Models;

namespace SpareParts.Client.Services
{
    public interface IInventoryService
    {
        [Get("/api/inventory")]
        Task<InventoryItemResponse> Get(int id);

        [Get("/api/inventory/index")]
        Task<InventoryItemListResponse> Index();

        [Get("/api/inventory/index-detail")]
        Task<InventoryItemDetailListResponse> IndexDetail(bool isCurrentOnly, int skip = 0, int? take = null);

        [Get("/api/inventory/report")]
        Task<HttpResponseMessage> Report(bool isCurrentOnly);

        [Post("/api/inventory")]
        Task<InventoryItemResponse> Post(InventoryItem InventoryItem);

        [Post("/api/inventory/post-list")]
        Task<InventoryItemListResponse> PostList(List<InventoryItem> inventoryItems);

        [Put("/api/inventory")]
        Task<InventoryItemResponse> Put(InventoryItem InventoryItem);

        [Delete("/api/inventory")]
        Task<InventoryItemResponse> Delete(int id);
    }
}
