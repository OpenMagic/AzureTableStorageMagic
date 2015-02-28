using System;

namespace AzureTableStorageMagic.RepositoryExceptions
{
    public class AddEntityException : RepositoryExceptionBase
    {
        public AddEntityException(string connectionString, string tableName, string partitionKey, string rowKey, Exception innerException)
            : base("Cannot add entity.", connectionString, tableName, partitionKey, rowKey, innerException)
        {
        }
    }
}