using System.Data.SqlClient;
using DatabaseAccess.Commands;
using DatabaseAccess.Commands.Abstract;
using DatabaseAccess.Commands.Models;

namespace DatabaseAccess.Abstract
{
    public abstract class AbstractDatabaseAccess<TDatabaseEntity> : AbstractReadOnlyDatabaseAccess<TDatabaseEntity>
        where TDatabaseEntity : DatabaseEntity, new()
    {
        public void ExecuteNonQuery(DatabaseCommand command)
        {
            ProcessNonQueryCommand(command);
        }

        public T[] ExecuteQuery<T>(DatabaseCommand command) where T : DatabaseEntity, new()
        {
            T[] result = ProcessCommand<T>(command);
            return result;
        }

        private void ProcessNonQueryCommand(DatabaseCommand command)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand sqlCommand = connection.CreateCommand())
            {
                sqlCommand.CommandText = command.FullCommandText;
                foreach (DatabaseParameter parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Insert<T>(T entity) where T : DatabaseEntity, new()
        {
            var insertCommand = new InsertCommand<T>(TableName, entity);
            ProcessNonQueryCommand(insertCommand);
        }

        public void Update<T>(T entity) where T : DatabaseEntity, new()
        {
            var updateCommand = new UpdateCommand<T>(TableName, entity);
            ProcessNonQueryCommand(updateCommand);
        }
    }
}
