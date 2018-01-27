using DatabaseAccess.Commands.Abstract;
using static System.String;

namespace DatabaseAccess
{
    public class SelectCommand : OrderableCommand
    {
        public SelectCommand(string table, string[] fields, string whereClause = null, string orderBy = null) : base(table, whereClause, orderBy)
        {
            Fields = fields;
        }

        public string[] Fields { get; }

        public override string FullCommandText =>
            $"{Type} {Join(",", Fields)} FROM {TableName} {WhereCondition} {OrderByCondition}";

        protected override string Type => "SELECT";
    }
}
