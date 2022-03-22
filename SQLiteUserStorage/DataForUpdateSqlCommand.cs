using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUserStorage
{
    public class DataForUpdateSqlCommand<T,K>
    {
        public T NewValue { get; set; }
        public string MutableAttribute { get; set; }
        public string WhereAttribute { get; set; }
        public K WhereValue { get; set; }

        public DataForUpdateSqlCommand(T newValue, K whereValue, string mutableAttribute, string whereAttribute)
        {
            NewValue = newValue;
            MutableAttribute = mutableAttribute;
            WhereAttribute = whereAttribute;
            WhereValue = whereValue;
        }
    }
}
