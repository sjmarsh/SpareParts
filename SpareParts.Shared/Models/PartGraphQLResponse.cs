namespace SpareParts.Shared.Models
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
