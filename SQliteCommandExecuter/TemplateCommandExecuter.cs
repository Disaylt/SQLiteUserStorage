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

        private void ExecuteCommand(string commandText, List<SqlParameters<object>>? sqlParameters = null, ParametersWriter? parametersWriter = null)
        {
            using var connection = new SqliteConnection(_connectionPath);
            try
            {
                connection.Open();
                var command = new SqliteCommand(commandText, connection);
                if(sqlParameters != null && parametersWriter != null)
                {
                    parametersWriter.Invoke(command, sqlParameters);
                }
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
            ExecuteCommand(commandText);
        }

        public void Insert(string tableName, List<SqlParameters<object>> insertParameters)
        {
            string[] columnsName = insertParameters
                .Select(x => x.ColumnName)
                .ToArray();
            string commandText = SqlCommandTextCreator.GetInsertCommand(tableName, columnsName);
            ExecuteCommand(commandText, insertParameters, SqlParametersHandler.WriteParameters);
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
            ExecuteCommand(commandText, allParameters, SqlParametersHandler.WriteParameters);
        }

        public void Delete(string tableName, List<SqlParameters<object>> deleteParameters)
        {
            string[] columnsName = deleteParameters.Select(x=> x.ColumnName).ToArray();
            string coomandText = SqlCommandTextCreator.GetDeleteCommand(tableName, columnsName);
            ExecuteCommand(coomandText, deleteParameters, SqlParametersHandler.WriteParameters);
        }
    }
}
