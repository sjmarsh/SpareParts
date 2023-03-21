using MediatR;
using SpareParts.API.Infrastructure;
using SpareParts.API.Services;
using SpareParts.Shared.Models;

namespace SpareParts.API.GraphQL
{
    public class Query
    {
        [UseFiltering]
        [AuthorizeByRoleHotChoc(Role.Administrator, Role.StocktakeUser)]
        public async Task<List<Shared.Models.Part>> GetParts([Service]IMediator mediator)
        {
            var partListResponse = await mediator.Send(new GetPartListRequest());
            return partListResponse.Items;
        }

        [AuthorizeByRoleHotChoc(Role.Administrator, Role.StocktakeUser)]
        public async Task<Shared.Models.Part> GetPartById([Service] IMediator mediator, int id)
        {
            var partResponse = await mediator.Send(new GetPartRequest(id));
            return partResponse.Value;
        }
    }
}
