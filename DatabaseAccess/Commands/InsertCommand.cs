using System;
using System.Linq;
using DatabaseAccess.Commands.Abstract;
using DatabaseAccess.Commands.Models;

namespace DatabaseAccess.Commands
{
    public class InsertCommand : DatabaseCommand
    {
        public InsertCommand(string table, string[] fields, object[] values) : base(table)
        {
            if (fields.Length != values.Length)
            {
                throw new ArgumentException("Number of fields is different to values");
            }

            _fields = fields;

            for (int i = 0; i < fields.Length; i++)
            {
                Parameters.Add(new DatabaseParameter(fields[i], values[i]));
            }
        }

        private readonly string[] _fields;

        public override string FullCommandText => $"{Type} INTO {TableName} {Fields} VALUES {Values}";

        protected string Fields => _fields != null && _fields.Any() ? $"({String.Join(",", _fields)})" : String.Empty;

        protected string Values => $"({String.Join(",", "@" + _fields)})";

        protected override string Type => "INSERT";
    }

    public class InsertCommand<T> : DatabaseCommand where T : DatabaseEntity, new()
    {
        public InsertCommand(string table, T dataEntity) : base(table)
        {
            PropertyData[] propertyInfo = AttributeHelper.DatabaseEntityInfo[typeof(T)].Where(data => data.ColumnData.IsInserted).ToArray();
            foreach (PropertyData propertyData in propertyInfo)
            {
                Parameters.Add(new DatabaseParameter("@" + propertyData.ColumnData.Column,propertyData.PropertyInfo.GetValue(dataEntity)));
            }

            _fields = propertyInfo.Select(p => p.ColumnData.Column).ToArray();
        }

        private readonly string[] _fields;

        public override string FullCommandText => $"{Type} INTO {TableName} {Fields} VALUES {Values}";

        protected string Fields => $"({String.Join(",", _fields)})";

        protected string Values => $"({String.Join(",", Parameters.Select(p => p.Key).ToArray())})";

        protected override string Type => "INSERT";
    }
}
