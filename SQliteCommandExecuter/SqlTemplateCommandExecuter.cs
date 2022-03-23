using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQliteCommandExecuter
{
    public class SqlTemplateCommandExecuter
    {
        private readonly string _connectionPath;
        public delegate void ExceptionPusher(Exception ex);
        public event ExceptionPusher? PushException;

        public SqlTemplateCommandExecuter(string dbName)
        {
            _connectionPath = $"Data Source={dbName}.db";
        }

        public SqlTemplateCommandExecuter(string dbName, ExceptionPusher exceptionPusher) : this(dbName)
        {
            PushException = exceptionPusher;
        }

        private string[] ReadParametersName(List<SqliteParameter> parameters)
        {
            string[] parametrsName = parameters
                .Select(x => x.ParameterName)
                .ToArray();
            return parametrsName;
        }

        private void ExecuteCommand(string commandText, List<SqliteParameter>? sqlParameters = null, ParametersWriter? parametersWriter = null)
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

        public void Insert(string tableName, List<SqliteParameter> insertParameters)
        {
            string[] parametersName = ReadParametersName(insertParameters);
            string commandText = SqlCommandTextCreator.GetInsertCommand(tableName, parametersName);
            ExecuteCommand(commandText, insertParameters, SqlParametersHandler.WriteParameters);
        }

        public void Update(string tableName, List<SqliteParameter> updateParameters, List<SqliteParameter> whereParameters)
        {
            string[] updateParametersName = ReadParametersName(updateParameters);
            string[] whereParametersName = ReadParametersName(whereParameters);
            string commandText = SqlCommandTextCreator.GetUpdateCommand(tableName, updateParametersName, whereParametersName);
            List<SqliteParameter> allParameters = whereParameters
                .Select(x=> new SqliteParameter($"Where{x.ParameterName}", x.Value))
                .Concat(updateParameters)
                .ToList();
            ExecuteCommand(commandText, allParameters, SqlParametersHandler.WriteParameters);
        }

        public void Delete(string tableName, List<SqliteParameter> deleteParameters)
        {
            string[] parametersName = ReadParametersName(deleteParameters);
            string coomandText = SqlCommandTextCreator.GetDeleteCommand(tableName, parametersName);
            ExecuteCommand(coomandText, deleteParameters, SqlParametersHandler.WriteParameters);
        }
    }
}
