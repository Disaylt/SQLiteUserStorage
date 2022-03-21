using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUserStorage
{
    internal static class SqlTableCreator
    {
        private static string GetColumns(string[] columns)
        {
            string columnsText = string.Empty;
            for(int num = 0; num < columns.Length; num++)
            {
                if(num == columns.Length -1)
                {
                    columnsText += columns[num];
                }
                else
                {
                    columnsText += $"{columns[num]}, ";
                }
            }
            return columnsText;

        }

        internal static string GetCommand(string tableName, string[] columns)
        {
            string createCommand = "CREATE TABLE";
            string columnsText = GetColumns(columns);
            return $"{createCommand} {tableName}({columnsText})";
        }
    }
}
