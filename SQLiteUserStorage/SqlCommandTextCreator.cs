using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUserStorage
{
    internal static class SqlCommandTextCreator
    {
        private static string TrimEndComma(string textWithComma)
        {
            string newText = textWithComma.Trim();
            newText = newText.Substring(0, newText.Length - 1);
            return newText;
        }

        private static string GetColumnsText(string[] columns)
        {
            string columnsText = string.Empty;
            foreach (string column in columns)
            {
                columnsText += $"{column}, ";
            }
            columnsText = TrimEndComma(columnsText);
            return columnsText;

        }

        private static (string columns, string values) GetColumnsAndValueText(Dictionary<string, string> columnsAndValue)
        {
            string columns = string.Empty;
            string values = string.Empty;
            foreach (var data in columnsAndValue)
            {
                columns += $"{data.Key}, ";
                values += $"{data.Value}, ";
            }
            columns = TrimEndComma(columns);
            values = TrimEndComma(values);
            return (columns, values);
        }

        internal static string GetCreateTableCommand(string tableName, string[] columns)
        {
            string createCommand = "CREATE TABLE";
            string columnsText = GetColumnsText(columns);
            return $"{createCommand} {tableName}({columnsText})";
        }

        internal static string GetInsertCommand(string tableName, Dictionary<string,string> columnsAndValue)
        {
            string createCommand = "INSERT INTO";
            (string columns, string values) = GetColumnsAndValueText(columnsAndValue);
            return $"{createCommand} {tableName} ({columns}) VALUES ({values})";
        }
    }
}
