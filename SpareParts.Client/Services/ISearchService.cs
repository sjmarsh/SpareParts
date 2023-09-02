using Refit;
using SpareParts.Shared.Models;

namespace SpareParts.Client.Services
{
    public interface ISearchService
    {
        // GraphQL
        [Post("/graphql")]
        Task<PartGraphQLResponsePaged> Search(GraphQLRequest request);
    }
}
