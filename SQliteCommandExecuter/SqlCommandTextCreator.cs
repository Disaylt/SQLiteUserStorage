using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQliteCommandExecuter
{
    public static class SqlCommandTextCreator
    {
        private static string TrimEndAttribute(string text, int attributeLenght)
        {
            text = text.TrimEnd();
            if(text.Length > attributeLenght)
            {
                text = text[0..^attributeLenght].Trim();
            }
            return text;
        }

        private static string GetColumnsText(string[] parametersName, string endAttribute, string key = "")
        {
            string columnsText = string.Empty;
            foreach(string parameterName in parametersName)
            {
                columnsText += $"{parameterName} = @{key}{parameterName}{endAttribute} ";
            }
            columnsText = TrimEndAttribute(columnsText, endAttribute.Length);
            return columnsText;
        }

        private static (string Parameters, string Values) GetColumnsAndValuesText(string[] parametersName)
        {
            string parameters = string.Empty;
            string values = string.Empty;
            foreach (string parameterName in parametersName)
            {
                parameters += $"{parameterName}, ";
                values += $"@{parameterName}, ";
            }
            parameters = TrimEndAttribute(parameters, 1);
            values = TrimEndAttribute(values, 1);
            return (parameters, values);
        }

        public static string GetCreateTableCommand(string tableName, string[] columns)
        {
            string command = "CREATE TABLE";
            string columnsText = GetColumnsAndValuesText(columns).Parameters;
            return $"{command} {tableName}({columnsText})";
        }

        public static string GetInsertCommand(string tableName, string[] parametersName)
        {
            string command = "INSERT INTO";
            (string columns, string values) = GetColumnsAndValuesText(parametersName);
            return $"{command} {tableName} ({columns}) VALUES ({values})";
        }

        public static string GetUpdateCommand(string table, string[] parametersName, string[] whereColumnsName)
        {
            string command = "UPDATE";
            string columnsNameText = GetColumnsText(parametersName, ",");
            string wherecolumnsName = GetColumnsText(whereColumnsName, " AND", "Where");
            return $"{command} {table} SET {columnsNameText} WHERE {wherecolumnsName}";
        }

        public static string GetDeleteCommand(string table, string[] parametersName)
        {
            string command = "DELETE FROM";
            string whereText = GetColumnsText(parametersName, " AND");
            return $"{command} {table} WHERE {whereText}";
        }

        public static string GetSelectCommand(string tableName)
        {
            string command = "SELECT";
            return $"{command} * FROM {tableName}";
        }

        public static string GetSelectCommand(string tableName, string[] whereParametersName)
        {
            string whereText = GetColumnsText(whereParametersName, " AND");
            string command = GetSelectCommand(tableName);
            return $"{command} WHERE {whereText}";
        }
    }
}
