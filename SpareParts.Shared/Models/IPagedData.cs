namespace SpareParts.Shared.Models
{
    public interface IPagedData<T>
    {
        List<T>? Items { get; set; }
        PageInfo? PageInfo { get; set; }
        public int TotalCount { get; set; }
    }

    public class PageInfo
    {
        public bool HasNextPage { get; set; }
    }
}
