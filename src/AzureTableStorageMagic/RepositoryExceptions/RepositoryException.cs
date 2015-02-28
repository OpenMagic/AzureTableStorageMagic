using System;
using System.Collections;
using System.Collections.Generic;
using EmptyStringGuard;
using NullGuard;

namespace AzureTableStorageMagic.RepositoryExceptions
{
    [Obsolete]
    public class RepositoryException : Exception
    {
        private readonly Dictionary<string, object> _data;

        public RepositoryException(string message, string connectionString, string tableName, [AllowNull, AllowEmpty] string partitionKey, [AllowNull, AllowEmpty] string rowKey, Exception innerException)
            : base(message, innerException)
        {
            _data = new Dictionary<string, object>
            {
                {"connectionString", connectionString},
                {"tableName", tableName},
                {"partitionKey", partitionKey},
                {"rowKey", rowKey}
            };

            throw new NotSupportedException();
        }

        public override IDictionary Data
        {
            get { return _data; }
        }
    }
}