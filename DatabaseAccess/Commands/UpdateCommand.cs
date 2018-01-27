using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.Commands.Abstract;
using DatabaseAccess.Commands.Models;
using static System.String;

namespace DatabaseAccess
{

    public class UpdateCommand<T> : ConditionalCommand where T : DatabaseEntity, new()
    {
        private readonly Dictionary<string, string> _fieldParameters = new Dictionary<string, string>();

        private readonly Dictionary<string, string> _conditions = new Dictionary<string, string>();

        public UpdateCommand(string table, T entity) : base(table, null)
        {
            PropertyData[] propertyInfo = AttributeHelper.GetPropertyData(typeof(T)).Where(data => data.ColumnData.IsInserted).ToArray();
            foreach (PropertyData propertyData in propertyInfo)
            {
                string parameterKey = "@" + propertyData.ColumnData.Column;
                Parameters.Add(new DatabaseParameter(parameterKey, propertyData.PropertyInfo.GetValue(entity)));

                _fieldParameters.Add(propertyData.ColumnData.Column, parameterKey);
            }

            PropertyData[] primaryKeyData = AttributeHelper.GetPropertyData(typeof(T)).Where(data => !data.ColumnData.IsInserted).ToArray();

            if (primaryKeyData.Length == 0)
            {
                throw new ArgumentException("Conditionless update is not allowed");
            }

            foreach (PropertyData data in primaryKeyData)
            {
                string parameterKey = "@" + data.ColumnData.Column;
                Parameters.Add(new DatabaseParameter(parameterKey, data.PropertyInfo.GetValue(entity)));

                _conditions.Add(data.ColumnData.Column, parameterKey);
            }
        }

        public override string FullCommandText => $"UPDATE {TableName} SET {FieldsToUpdate} {WhereClause}";

        protected override string WhereClause => $"WHERE {Join(",", _conditions.Select(c => $"{c.Key} = {c.Value}").ToArray())}";

        private string FieldsToUpdate => Join(",", _fieldParameters.Select(p => $"{p.Key} = {p.Value}"));

        protected override string Type => "UPDATE";
    }
}
