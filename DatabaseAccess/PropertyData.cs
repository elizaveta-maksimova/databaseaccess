using System.Reflection;

namespace DatabaseAccess
{
    public class PropertyData
    {
        public PropertyData(PropertyInfo propertyInfo, DataColumnAttribute columnData)
        {
            PropertyInfo = propertyInfo;
            ColumnData = columnData;
        }

        public PropertyInfo PropertyInfo { get; set; }

        public DataColumnAttribute ColumnData { get; set; }
    }
}
