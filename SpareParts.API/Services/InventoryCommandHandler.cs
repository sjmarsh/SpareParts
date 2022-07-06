using MediatR;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public record CreateInventoryItemCommand : IRequest<InventoryItemResponse>
    {
        public CreateInventoryItemCommand(InventoryItem iventoryItem)
        {
            IventoryItem = iventoryItem;
        }

        public InventoryItem? IventoryItem { get; }
    }

    public class CreateInventoryItemCommandHandler : BaseHandler, IRequestHandler<CreateInventoryItemCommand, InventoryItemResponse>
    {
        public CreateInventoryItemCommandHandler(IDataService dataService, ILogger<CreateInventoryItemCommandHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<InventoryItemResponse> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            try
            {


                return await _dataService.CreateItem<InventoryItemResponse, Entities.InventoryItem, InventoryItem>(request.IventoryItem, cancellationToken);
            }
            catch(Exception ex)
            {
                return ReturnAndLogException<InventoryItemResponse, InventoryItem>("An error occured while creating Inventory Item.", ex);
            }
        }
    }

    public record UpdateInventoryItemCommand : IRequest<InventoryItemResponse>
    {
        public UpdateInventoryItemCommand(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
        }

        public InventoryItem InventoryItem { get; }
    }

    public class UpdateInventoryItemCommandHandler : BaseHandler, IRequestHandler<UpdateInventoryItemCommand, InventoryItemResponse>
    {
        public UpdateInventoryItemCommandHandler(IDataService dataService, ILogger<UpdateInventoryItemCommandHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<InventoryItemResponse> Handle(UpdateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.UpdateItem<InventoryItemResponse, Entities.InventoryItem, InventoryItem>(request.InventoryItem, cancellationToken);
            }
            catch (Exception ex)
            {
                return ReturnAndLogException<InventoryItemResponse, InventoryItem>("An error occurred while updating Inventory Item.", ex);
            }
        }
    }

    public record DeleteInventoryItemCommand : IRequest<InventoryItemResponse>
    {
        public DeleteInventoryItemCommand(int inventoryItemID)
        {
            InventoryItemID = inventoryItemID;
        }

        public int InventoryItemID { get; }
    }

    public class DeleteInventoryItemRequestHandler : BaseHandler, IRequestHandler<DeleteInventoryItemCommand, InventoryItemResponse>
    {
        public DeleteInventoryItemRequestHandler(IDataService dataService, ILogger<DeleteInventoryItemRequestHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<InventoryItemResponse> Handle(DeleteInventoryItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.DeleteItem<InventoryItemResponse, Entities.InventoryItem, InventoryItem>(request.InventoryItemID, cancellationToken);
            }
            catch (Exception ex)
            {
                return ReturnAndLogException<InventoryItemResponse, InventoryItem>("An error occurred while deleting Inventory Item.", ex);
            }
        }
    }

}
