using System.Reflection;
using SpareParts.Shared.Extensions;

namespace SpareParts.Client.Shared.Components.DataGrid
{
    public class DataRow<T>
    {
        private readonly T _sourceItem;
        private readonly List<ColumnHeader>? _columnList;

        public DataRow(T sourceItem, List<ColumnHeader>? columnList = null)
        {
            _sourceItem = sourceItem;
            _columnList = columnList;
        }

        private Dictionary<string, string>? _data;
        public Dictionary<string, string> Data
        {
            get
            {
                _data ??= GetData();
                return _data;
            }
        }

        public bool IsDetailsVisible { get; set; }

        private List<DataRowDetail>? _detailRows;
        public List<DataRowDetail>? DetailRows
        {
            get
            {
                _detailRows ??= GetDetailRows();
                return _detailRows;
            }
        }

        private Dictionary<string, string> GetData()
        {
            var data = new Dictionary<string, string>();

            if (_sourceItem != null && _columnList != null)
            {
                var props = _sourceItem.GetType().GetProperties();

                foreach (var prop in props)
                {
                    var columnName = prop.Name;

                    if (_columnList.FirstOrDefault(c => c.ColumnName == columnName && c.ParentColumnName == null) != null)
                    {
                        var strvalue = prop.GetStringValueOrDefault(_sourceItem);
                        data.Add(columnName, strvalue);
                    }
                }
            }

            return data;
        }

        private List<DataRowDetail> GetDetailRows()
        {
            var detailRows = new List<DataRowDetail>();
            var props = _sourceItem?.GetType().GetProperties();
            if (props != null && _columnList != null)
            {
                foreach (var prop in props)
                {
                    var columnName = prop.Name;
                    if (_columnList.FirstOrDefault(c => c.ColumnName == columnName && c.ParentColumnName == null) == null)
                    {
                        detailRows.Add(GetRowDetails(prop, _sourceItem, columnName));
                    }
                }
            }

            return detailRows;
        }


        private DataRowDetail GetRowDetails(PropertyInfo prop, T item, string columnName)
        {
            var type = prop.PropertyType;
            var value = prop.GetValue(item);

            if (value != null)
            {
                if (type.IsGenericType)
                {
                    var genType = prop.PropertyType.GetGenericArguments()[0];
                    var genTypeProps = genType.GetProperties();

                    if (value.GetType().IsListType())
                    {
                        var listItems = (System.Collections.IEnumerable)value;
                        var listData = new List<Dictionary<string, string>>();
                        foreach (var listItem in listItems)
                        {
                            var listRow = new Dictionary<string, string>();
                            foreach (var genTypeProp in genTypeProps)
                            {
                                var lstItemName = genTypeProp.Name;
                                var listItemValue = genTypeProp.GetStringValueOrDefault(listItem);
                                if (_columnList != null && _columnList.FirstOrDefault(c => c.ColumnName == lstItemName && c.ParentColumnName == columnName) != null)
                                {
                                    listRow.Add(lstItemName, listItemValue);
                                }
                            }
                            listData.Add(listRow);
                        }
                        return new DataRowDetail(columnName, listData);
                    }

                    // else not a list  // TODO implement


                }

            }

            return new DataRowDetail(columnName, new List<Dictionary<string, string>>());
        }
    }

    public class DataRowDetail
    {
        public DataRowDetail(string detailHeader, List<Dictionary<string, string>> data)
        {
            DetailHeader = detailHeader;
            Data = data;
        }

        public string DetailHeader { get; set; }
        public List<Dictionary<string, string>> Data { get; set; }
    }
}
