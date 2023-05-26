using Refit;
using SpareParts.Client.Features.Search;

namespace SpareParts.Client.Services
{
    public interface ISearchService
    {
        // GraphQL
        [Post("/graphql")]
        Task<PartGraphQLResponse> Search(GraphQLRequest request);
    }
}
