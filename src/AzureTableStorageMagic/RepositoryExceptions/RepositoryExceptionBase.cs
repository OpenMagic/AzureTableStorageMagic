using System;
using System.Collections;
using System.Collections.Generic;

namespace AzureTableStorageMagic.RepositoryExceptions
{
    public class RepositoryExceptionBase : Exception
    {
        private readonly Dictionary<string, object> _data;

        public RepositoryExceptionBase()
        {
            throw new NotSupportedException("todo: inheritors should call ctor with parameters.");
        }

        protected RepositoryExceptionBase(string message, string connectionString, string tableName, string partitionKey, string rowKey, Exception innerException) :
            base(message, innerException)
        {
            _data = new Dictionary<string, object>
            {
                {"connectionString", connectionString},
                {"tableName", tableName},
                {"partitionKey", partitionKey},
                {"rowKey", rowKey}
            };
        }

        protected RepositoryExceptionBase(string message, string connectionString, string tableName, Exception innerException) :
            base(message, innerException)
        {
            _data = new Dictionary<string, object>
            {
                {"connectionString", connectionString},
                {"tableName", tableName}
            };
        }

        protected RepositoryExceptionBase(string message, string connectionString, Exception innerException) :
            base(message, innerException)
        {
            _data = new Dictionary<string, object>
            {
                {"connectionString", connectionString},
            };
        }

        public override IDictionary Data
        {
            get { return _data; }
        }
    }
}