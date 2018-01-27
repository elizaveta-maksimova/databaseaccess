using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using DatabaseAccess.Commands.Abstract;
using DatabaseAccess.Commands.Models;

namespace DatabaseAccess.Abstract
{
    public abstract class AbstractReadOnlyDatabaseAccess<TDatabaseEntity> where TDatabaseEntity : DatabaseEntity, new()
    {
        public abstract string TableName { get; }
        public abstract string ConnectionString { get; }

        public TDatabaseEntity GetRecord(Expression<Func<TDatabaseEntity, bool>> expression = null)
        {
            TDatabaseEntity[] result = GetRecords(expression);

            return result.Single();
        }

        public TDatabaseEntity[] GetRecords(Expression<Func<TDatabaseEntity, bool>> expression = null)
        {
            string whereClause = string.Empty;
            if (expression != null)
            {
                BinaryExpression e = expression.Body as BinaryExpression;
                whereClause = ExpressionParser.GetWhereClause(e);
            }

            return ProcessSelect<TDatabaseEntity>(GetFields<TDatabaseEntity>(), whereClause);
        }

        public T GetRecord<T>(Expression<Func<TDatabaseEntity, bool>> expression) where T : DatabaseEntity, new()
        {
            T[] result = GetRecords<T>(expression);

            return result.Any() ? result.Single() : null;
        }

        public T[] GetRecords<T>(Expression<Func<TDatabaseEntity, bool>> expression = null) where T : DatabaseEntity, new()
        {
            string whereClause = string.Empty;
            if (expression != null)
            {
                BinaryExpression e = expression.Body as BinaryExpression;
                whereClause = ExpressionParser.GetWhereClause(e);
            }
            
            return ProcessSelect<T>(GetFields<T>(), whereClause);
        }

        private string[] GetFields<T>()
        {
            PropertyData[] fieldsInfo = AttributeHelper.GetPropertyData(typeof(T));
            return fieldsInfo.Select(i => i.ColumnData.Column).ToArray();
        }

        private T[] ProcessSelect<T>(string[] fields, string whereClause) where T : DatabaseEntity, new()
        {
            return ProcessCommand<T>(new SelectCommand(TableName, fields, whereClause));
        }

        protected T[] ProcessCommand<T>(DatabaseCommand command) where T : DatabaseEntity, new()
        {
            T[] results;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand sqlCommand = connection.CreateCommand())
            {
                sqlCommand.CommandText = command.FullCommandText;

                foreach (DatabaseParameter parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                connection.Open();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    results = ReadEntities<T>(reader);
                }

                connection.Close();
            }


            return results.ToArray();
        }

        private T[] ReadEntities<T>(SqlDataReader reader) where T : new()
        {
            List<T> results = new List<T>();
            while (reader.Read())
            {
                T readEntity = new T();
                foreach (PropertyData propertyData in AttributeHelper.GetPropertyData(typeof(T)))
                {
                    propertyData.PropertyInfo.SetValue(readEntity, GetReadValue(reader[propertyData.ColumnData.Column], propertyData.PropertyInfo.PropertyType));
                }

                results.Add(readEntity);
            }

            return results.ToArray();
        }

        private object GetReadValue(object o, Type t)
        {
            if (o is DBNull)
            {
                return null;
            }

            if (t == typeof(DateTime?))
            {
                return (DateTime?)o;
            }

            return Convert.ChangeType(o, t);
        }
    }
}
