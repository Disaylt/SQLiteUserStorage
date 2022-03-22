using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUserStorage
{
    public static class SQLiteCommandExecuter
    {
        private static string _connectionPath = "Data Source=default.db";
        public delegate void ExceptionPusher(Exception ex);
        public static event ExceptionPusher? PushException;
        public static string FileNameDB
        {
            set
            {
                _connectionPath = $"Data Source={value}.db";
            }
        }

        private static void AddParametrs(SqliteCommand command, Dictionary<string, object?> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
        }

        private static void ExcecuteCommand(string commandText)
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
                if (PushException != null)
                {
                    PushException.Invoke(ex);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private static void ExcecuteCommand(string commandText, Dictionary<string, object?> parameters)
        {
            using var connection = new SqliteConnection(_connectionPath);
            try
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(commandText, connection);
                AddParametrs(command, parameters);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (PushException != null)
                {
                    PushException.Invoke(ex);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public static void CrateTable(string tableName, params string[] columns)
        {
            string commandText = SqlCommandTextCreator.GetCreateTableCommand(tableName, columns);
            ExcecuteCommand(commandText);
        }

        public static void Insert(string tableName, Dictionary<string, string> columnsAndValue)
        {
            string commandText = SqlCommandTextCreator.GetInsertCommand(tableName, columnsAndValue);
            ExcecuteCommand(commandText);
        }

        public static void Update<T, K>(string tableName, DataForUpdateSqlCommand<T, K> dataForUpdate)
        {
            string commandText = SqlCommandTextCreator.GetUpdateCommand(tableName, dataForUpdate.MutableAttribute, dataForUpdate.WhereAttribute);
            Dictionary<string, object?> parameters = new Dictionary<string, object?>
            {
                {"@value", dataForUpdate.NewValue },
                {"@whereValue", dataForUpdate.WhereValue }
            };
            ExcecuteCommand(commandText, parameters);
        }
    }
}
