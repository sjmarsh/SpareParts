namespace SpareParts.Shared.Models
{
    public class ResponseBase<T> where T : ModelBase
    {
        public T? Value { get; set; }
        public bool HasError { get; set; }
        public string? Message { get; set; }
    }

    public class ResponseListBase<T> where T : ModelBase
    {
        public List<T>? Items { get; set; }
        public int TotalItemCount { get; set; }
        public bool HasError { get; set; }
        public string? Message { get; set; }
    }
}
