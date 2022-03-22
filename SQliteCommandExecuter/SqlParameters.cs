using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQliteCommandExecuter
{
    public class SqlParameters<T>
    {
        public string ColumnName { get; set; }
        public T Value { get; set; }
        public SqlParameters(string columnName, T value)
        {
            ColumnName = columnName;
            Value = value;
        }
    }
}
