using Microsoft.WindowsAzure.Storage;

namespace AzureTableStorageMagic.RepositoryExceptions
{
    public class ServerNotFoundException : RepositoryExceptionBase
    {
        public ServerNotFoundException(string connectionString, StorageException innerException) : base("Cannot find server.", connectionString, innerException)
        {
        }
    }
}