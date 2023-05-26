using SpareParts.Shared.Models;

namespace SpareParts.Client.Features.Search
{
    public class PartGraphQLResponse
    {
        public MyGraphQLData? Data { get; set; }
    }

    public class MyGraphQLData
    {
        public List<Part>? Parts { get; set; }
    }
}
