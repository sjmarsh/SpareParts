namespace SpareParts.Client.Shared.Components.DataGrid
{
    public record ColumnHeader
    {
        public ColumnHeader(string columnName, string? parentColumnName = null)
        {
            ColumnName = columnName;
            ParentColumnName = parentColumnName;
        }
        public string ColumnName { get; private set; }
        public string? ParentColumnName { get; private set; }
    }
}
