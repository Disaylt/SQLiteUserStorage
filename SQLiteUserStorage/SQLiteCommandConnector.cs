using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUserStorage
{
    public static class SQLiteCommandConnector
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

    }
}
