using MediatR;
using SpareParts.API.Services;

namespace SpareParts.API.GraphQL
{
    public class Query
    {
        [UseFiltering]
        public async Task<List<Shared.Models.Part>> GetParts([Service]IMediator mediator)
        {
            var partListResponse = await mediator.Send(new GetPartListRequest());
            return partListResponse.Items;
        }

        public async Task<Shared.Models.Part> GetPartById([Service] IMediator mediator, int id)
        {
            var partResponse = await mediator.Send(new GetPartRequest(id));
            return partResponse.Value;
        }
    }
}
