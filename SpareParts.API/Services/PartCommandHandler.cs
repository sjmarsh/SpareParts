using MediatR;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public record CreatePartCommand : IRequest<PartResponse>
    {
        public CreatePartCommand(Part part)
        {
            Part = part;
        }

        public Part? Part { get; }
    }

    public class CreatePartCommandHandler : BaseHandler, IRequestHandler<CreatePartCommand, PartResponse>
    {
        public CreatePartCommandHandler(IDataService dataService, ILogger<CreatePartCommandHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<PartResponse> Handle(CreatePartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.CreateItem<PartResponse, Entities.Part, Part>(request.Part, cancellationToken);
            }
            catch(Exception ex)
            {        
                return ReturnAndLogException<PartResponse, Part>("An error occured while creating Part.", ex);
            }            
        }
    }

    public record UpdatePartCommand : IRequest<PartResponse>
    {
        public UpdatePartCommand(Part part)
        {
            Part = part;
        }

        public Part? Part { get; }
    }

    public class UpdatePartCommandHandler : BaseHandler, IRequestHandler<UpdatePartCommand, PartResponse>
    {
        public UpdatePartCommandHandler(IDataService dataService, ILogger<UpdatePartCommandHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<PartResponse> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var referencedCollectionsToUpdate = new[] { nameof(Part.Attributes) };
                return await _dataService.UpdateItem<PartResponse, Entities.Part, Part>(request.Part, cancellationToken, referencedCollectionsToUpdate);
            }
            catch(Exception ex)
            {
                return ReturnAndLogException<PartResponse, Part>("An error occurred while updating Part.", ex);
            }
        }
    }

    public record DeletePartCommand : IRequest<PartResponse>
    {
        public DeletePartCommand(int partID)
        {
            PartID = partID;
        }

        public int PartID { get; }
    }

    public class DeletePartRequestHandler : BaseHandler, IRequestHandler<DeletePartCommand, PartResponse>
    {
        public DeletePartRequestHandler(IDataService dataService, ILogger<DeletePartRequestHandler> logger) : base(dataService, logger)
        {
        }

        public async Task<PartResponse> Handle(DeletePartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.DeleteItem<PartResponse, Entities.Part, Part>(request.PartID, cancellationToken);
            }
            catch(Exception ex)
            {
                var relatedMessaged = CheckForRelatedInventoryItems(request.PartID);
                return ReturnAndLogException<PartResponse, Part>($"An error occurred while deleting Part.{relatedMessaged}", ex);
            }            
        }

        private string CheckForRelatedInventoryItems(int partId)
        {
            var hasRelatedItems = _dataService.HasRelatedItems<Entities.InventoryItem>(i => i.PartID == partId);
            return hasRelatedItems ? " Part has associated Inventory Items and cannot be deleted." : "";
        }
    }    
}
