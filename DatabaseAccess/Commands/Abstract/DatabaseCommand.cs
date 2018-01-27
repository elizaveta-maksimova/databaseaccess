using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseAccess.Commands.Models;

namespace DatabaseAccess.Commands.Abstract
{
    public abstract class DatabaseCommand
    {
        protected DatabaseCommand(string table = null)
        {
            TableName = table;
            Parameters = new List<DatabaseParameter>();
        }

        public List<DatabaseParameter> Parameters { get; }

        public abstract string FullCommandText { get; }

        protected virtual string Type { get; }

        protected string TableName { get; }
    }

    public abstract class OrderableCommand : ConditionalCommand
    {
        protected OrderableCommand(string table, string whereClause, string orderBy = null, SortOrder order = SortOrder.Unspecified) : base(table, whereClause)
        {
            _orderBy = orderBy;
            _order = order;
        }

        private readonly string _orderBy;
        private readonly SortOrder _order;

        protected string OrderByCondition => $"{(string.IsNullOrEmpty(_orderBy) ? string.Empty : $"ORDER BY {_orderBy} {(_order == SortOrder.Descending ? "DESC" : "ASC")}")}";
    }

    public abstract class ConditionalCommand : DatabaseCommand
    {
        protected virtual string WhereClause { get; set; }

        protected ConditionalCommand(string table, string whereClause) : base(table)
        {
            WhereClause = whereClause;
        }

        protected string WhereCondition => $"{(string.IsNullOrEmpty(WhereClause) ? string.Empty : "WHERE " + WhereClause)}";
    }
}
