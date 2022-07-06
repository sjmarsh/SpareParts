using MediatR;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public record GetPartRequest : IRequest<PartResponse>
    {
        public GetPartRequest(int partID)
        {
            PartID = partID;
        }

        public int PartID { get; }
    }

    public class GetPartRequestHandler : BaseHandler, IRequestHandler<GetPartRequest, PartResponse>
    {
        public GetPartRequestHandler(IDataService dataService, ILogger<GetPartRequestHandler> logger) 
            : base(dataService, logger)
        {
        }

        public async Task<PartResponse> Handle(GetPartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.GetItem<PartResponse, Entities.Part, Part>(request.PartID, cancellationToken);
            }
            catch(Exception ex)
            {
                return ReturnAndLogException<PartResponse, Part>("An error occurred while finding Part.", ex);
            }        
        }
    }

    public record GetPartListRequest : IRequest<PartListResponse>
    {
    }

    public class GetPartListRequestHandler : BaseHandler, IRequestHandler<GetPartListRequest, PartListResponse>
    {
        
        public GetPartListRequestHandler(IDataService dataService, ILogger<GetPartListRequestHandler> logger) 
            : base(dataService, logger)
        {    
        }

        public async Task<PartListResponse> Handle(GetPartListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _dataService.GetList<PartListResponse, Entities.Part, Part>(cancellationToken);
            }
            catch(Exception ex)
            {
                return ReturnListAndLogException<PartListResponse, Part>("An error occurred while getting Parts.", ex);
            }
        }
    }

}
