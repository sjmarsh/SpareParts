using MediatR;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public record GetInventoryItemRequest : IRequest<InventoryItemResponse>
    {
        public GetInventoryItemRequest(int inventoryItemID)
        {
            InventoryItemID = inventoryItemID;
        }

        public int InventoryItemID { get; }
    }

    public class GetInventoryItemRequestHandler : BaseHandler, IRequestHandler<GetInventoryItemRequest, InventoryItemResponse>
    {
        public GetInventoryItemRequestHandler(IDataService dataService, ILogger<GetInventoryItemRequestHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<InventoryItemResponse> Handle(GetInventoryItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.GetItem<InventoryItemResponse, Entities.InventoryItem, InventoryItem>(request.InventoryItemID, cancellationToken);
            }
            catch (Exception ex)
            {
                return ReturnAndLogException<InventoryItemResponse, InventoryItem>("An error occurred while finding Inventory Item.", ex);
            }
        }
    }

    public record GetInventoryItemListRequest : IRequest<InventoryItemListResponse>
    {
    }

    public class GetInventoryItemListRequestHandler : BaseHandler, IRequestHandler<GetInventoryItemListRequest, InventoryItemListResponse>
    {

        public GetInventoryItemListRequestHandler(IDataService dataService, ILogger<GetInventoryItemListRequestHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<InventoryItemListResponse> Handle(GetInventoryItemListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.GetList<InventoryItemListResponse, Entities.InventoryItem, InventoryItem>(cancellationToken);
            }
            catch (Exception ex)
            {
                return ReturnListAndLogException<InventoryItemListResponse, InventoryItem>("An error occurred while getting Inventory Items.", ex);
            }
        }
    }
}
