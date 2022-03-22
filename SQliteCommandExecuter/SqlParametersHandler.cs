using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQliteCommandExecuter
{
    internal delegate void ParametersWriter(SqliteCommand command, List<SqlParameters<object>> parameters);
    internal static class SqlParametersHandler
    {
        internal static void WriteParameters(SqliteCommand command, List<SqlParameters<object>> parameters)
        {
            foreach(var parameter in parameters)
            {
                command.Parameters.AddWithValue($"@{parameter.ColumnName}", parameter.Value);
            }
        }
    }
}
