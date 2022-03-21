using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUserStorage
{
    internal static class SqlTableCreator
    {
        internal static string GetCommand(string tableName, string[] columns)
        {
            string createCommand = "CREATE TABLE";
            string allColumns = string.Empty;
            foreach (string column in columns)
            {
                allColumns += $"{column}, ";
            }
            return $"{createCommand} {tableName}({allColumns})";
        }
    }
}
