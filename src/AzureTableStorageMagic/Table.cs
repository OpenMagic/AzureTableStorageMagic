using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic
{
    public class Table : ITable
    {
        public Task CreateTableIfNotExists(string connectionString, string tableName)
        {
            return GetCloudTable(connectionString, tableName).CreateIfNotExistsAsync();
        }

        public Task DeleteTableIfExists(string connectionString, string tableName)
        {
            return GetCloudTable(connectionString, tableName).DeleteIfExistsAsync();
        }

        public static CloudTable GetCloudTable(string connectionString, string tableName)
        {
            var storage = CloudStorageAccount.Parse(connectionString);
            var client = storage.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);

            return table;
        }
    }
}