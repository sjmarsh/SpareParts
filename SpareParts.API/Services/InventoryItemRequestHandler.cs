using SpareParts.API.Infrastructure.SimpleMediator;
using Microsoft.EntityFrameworkCore;
using SpareParts.API.Data;
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

    public record GetInventoryItemDetailListRequest : IRequest<InventoryItemDetailListResponse>
    {
        public bool IsCurrentOnly { get; set; }
        public int Skip { get; set; }
        public int? Take { get; set; }
    }

    public class GetInventoryItemDetailListRequestHandler : BaseHandler, IRequestHandler<GetInventoryItemDetailListRequest, InventoryItemDetailListResponse>
    {
        private readonly SparePartsDbContext _dbContext;

        public GetInventoryItemDetailListRequestHandler(SparePartsDbContext dbContext, IDataService dataService, ILogger<GetInventoryItemDetailListRequestHandler> logger) : base(dataService, logger)
        {            
            _dbContext = dbContext;
        }

        public async Task<InventoryItemDetailListResponse> Handle(GetInventoryItemDetailListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var parts = _dbContext.Parts.OrderBy(p => p.Name).AsQueryable();
                
                if(request.IsCurrentOnly)
                {
                    parts = parts.Where(p => p.StartDate.Date <= DateTime.Today && (!p.EndDate.HasValue || p.EndDate.Value.Date >= DateTime.Today));
                }

                var inventoryItems = _dbContext.InventoryItems.OrderByDescending(i => i.DateRecorded).AsQueryable();
                
                if (request.IsCurrentOnly)
                {
                    var recentItems = inventoryItems.GroupBy(i => i.PartID).Select(g => new { PartID = g.Key, DateRecorded = g.Max(x => x.DateRecorded) });
                    inventoryItems = inventoryItems.Where(i => recentItems.FirstOrDefault(r => r.PartID == i.PartID && r.DateRecorded == i.DateRecorded) != null);
                }

                var query = parts.Join(inventoryItems, p => p.ID, i => i.PartID, (p, i) => new InventoryItemDetail(i.ID, p.ID, p.Name, i.Quantity, i.DateRecorded));
                
                var totalCount = query.Count();
                var take = request.Take ?? totalCount;

                var detailItems = await query.Skip(request.Skip).Take(take).ToListAsync();
                                
                return new InventoryItemDetailListResponse { Items = detailItems, TotalItemCount = totalCount };
            }
            catch(Exception ex)
            {
                return ReturnListAndLogException<InventoryItemDetailListResponse, InventoryItemDetail>("Error occurred while getting Inventory Item Details.", ex);
            }
        }
    }
}
