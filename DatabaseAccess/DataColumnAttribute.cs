using System;

namespace DatabaseAccess
{

    [AttributeUsage(AttributeTargets.Property)]
    public class DataColumnAttribute : Attribute
    {
        public DataColumnAttribute(string column, bool isPrimaryKey = false, bool isInserted = true)
        {
            Column = column;
            IsPrimaryKey = isPrimaryKey;
            IsInserted = isInserted;
        }

        public string Column { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsInserted { get; set; }
    }
}
