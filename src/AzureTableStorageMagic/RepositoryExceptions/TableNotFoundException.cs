using Microsoft.WindowsAzure.Storage;

namespace AzureTableStorageMagic.RepositoryExceptions
{
    public class TableNotFoundException : RepositoryExceptionBase
    {
        public TableNotFoundException(string connectionString, string tableName, StorageException innerException)
            : base("Table not found", connectionString, tableName, innerException)
        {
        }
    }
}