using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQliteCommandExecuter
{
    public class TemplateCommandExecuter
    {
        private readonly string _connectionPath;
        public delegate void ExceptionPusher(Exception ex);
        public event ExceptionPusher? PushException;
        public TemplateCommandExecuter(string dbName)
        {
            _connectionPath = $"Data Source={dbName}.db";
        }

        public TemplateCommandExecuter(string dbName, ExceptionPusher exceptionPusher) : this(dbName)
        {
            PushException = exceptionPusher;
        }

        private static void AddParametrs(SqliteCommand command, Dictionary<string, object?> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
        }

        private void ExcecuteCommand(string commandText)
        {
            using var connection = new SqliteConnection(_connectionPath);
            try
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                PushException?.Invoke(ex);
            }
            finally
            {
                connection.Close();
            }
        }

        private void ExcecuteCommand(string commandText, List<SqlParameters<object>> sqlParameters, ParametersWriter parametersWriter)
        {
            using var connection = new SqliteConnection(_connectionPath);
            try
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(commandText, connection);
                parametersWriter.Invoke(command, sqlParameters);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                PushException?.Invoke(ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public void CrateTable(string tableName, params string[] columns)
        {
            string commandText = SqlCommandTextCreator.GetCreateTableCommand(tableName, columns);
            ExcecuteCommand(commandText);
        }

        public void Insert(string tableName, List<SqlParameters<object>> parameters)
        {
            string[] columnsName = parameters
                .Select(x => x.ColumnName)
                .ToArray();
            string commandText = SqlCommandTextCreator.GetInsertCommand(tableName, columnsName);
            ExcecuteCommand(commandText, parameters, SqlParametersHandler.WriteParameters);
        }

        public void Update(string tableName, List<SqlParameters<object>> updateParameters, List<SqlParameters<object>> whereParameters)
        {
            string[] columnsName = updateParameters.Select(x => x.ColumnName).ToArray();
            string[] whereColumnsName = whereParameters.Select(x => x.ColumnName).ToArray();
            string commandText = SqlCommandTextCreator.GetUpdateCommand(tableName, columnsName, whereColumnsName);
            List<SqlParameters<object>> allParameters = whereParameters
                .Select(x=> new SqlParameters<object>($"Where{x.ColumnName}", x.Value))
                .Concat(updateParameters)
                .ToList();
            ExcecuteCommand(commandText, allParameters, SqlParametersHandler.WriteParameters);
        }
    }
}
