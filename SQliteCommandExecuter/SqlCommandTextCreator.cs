using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQliteCommandExecuter
{
    internal static class SqlCommandTextCreator
    {
        private static string TrimEndAttribute(string text, int attributeLenght)
        {
            text = text.TrimEnd();
            text = text.Substring(0, text.Length - attributeLenght).Trim();
            return text;
        }

        private static string GetColumnsText(string[] columnsName, string endAttribute, string key = "")
        {
            string columnsText = string.Empty;
            foreach(string columnName in columnsName)
            {
                columnsText += $"{columnsName} = @{key}{columnsName}{endAttribute} ";
            }
            columnsText = TrimEndAttribute(columnsText, endAttribute.Length);
            return columnsText;
        }

        private static (string columns, string values) GetColumnsAndValuesText(string[] columnsName)
        {
            string columns = string.Empty;
            string values = string.Empty;
            foreach (string columnName in columnsName)
            {
                columns += $"{columnName}, ";
                values += $"@{columnName}, ";
            }
            columns = TrimEndAttribute(columns, 1);
            values = TrimEndAttribute(values, 1);
            return (columns, values);
        }

        internal static string GetCreateTableCommand(string tableName, string[] columns)
        {
            string command = "CREATE TABLE";
            string columnsText = GetColumnsText(columns, ",");
            return $"{command} {tableName}({columnsText})";
        }

        internal static string GetInsertCommand(string tableName, string[] columnsName)
        {
            string command = "INSERT INTO";
            (string columns, string values) = GetColumnsAndValuesText(columnsName);
            return $"{command} {tableName} ({columns}) VALUES ({values})";
        }

        internal static string GetUpdateCommand(string table, string[] columnsName, string[] whereColumnsName)
        {
            string command = "UPDATE";
            string columnsNameText = GetColumnsText(columnsName, " AND");
            string wherecolumnsName = GetColumnsText(whereColumnsName, " AND", "Where");
            return $"{command} {table} SET {columnsNameText} WHERE {wherecolumnsName}";
        }

        internal static string GetDeleteCommand(string table, string[] columnsName)
        {
            string command = "DELETE FROM";
            string whereText = GetColumnsText(columnsName, " AND");
            return $"{command} {table} WHERE {whereText}";
        }
    }
}
