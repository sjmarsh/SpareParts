namespace SpareParts.Shared.Models
{
    // non-paged response
    public class PartGraphQLResponse
    {
        public MyGraphQLData? Data { get; set; }
    }

    public class MyGraphQLData
    {
        public List<Part>? Parts { get; set; }
    }


    // pagination enabled respose
    public class PartGraphQLResponsePaged
    {
        public PartGraphQLResponsePagedData? Data { get; set; }
    }

    public class PartGraphQLResponsePagedData
    {
        public PartGraphQLResponsePagedItems? Parts {  get; set; }
    }

    public class PartGraphQLResponsePagedItems : IPagedData<PartGraphQLObject>
    {
        public List<PartGraphQLObject>? Items { get; set; }
        public PageInfo? PageInfo { get; set; }
        public int TotalCount { get; set; }
    }
}
