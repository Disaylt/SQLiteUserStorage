using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQliteCommandExecuter
{
    public class TemplateCommandExecuter
    {
        private readonly string _connectionPath;
        public delegate void ExceptionPusher(Exception ex);
        public event ExceptionPusher? PushException;
        public TemplateCommandExecuter(string dbName)
        {
            _connectionPath = $"Data Source={dbName}.db";
        }

        public TemplateCommandExecuter(string dbName, ExceptionPusher exceptionPusher) : this(dbName)
        {
            PushException = exceptionPusher;
        }
    }
}
