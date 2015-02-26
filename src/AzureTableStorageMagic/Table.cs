using System.Threading.Tasks;
using Common.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic
{
    public class Table : ITable
    {
        private static readonly ILog Log = LogManager.GetLogger<Table>();

        private readonly string _connectionString;
        private readonly string _tableName;

        public Table(string connectionString, string tableName)
        {
            // todo: validate tableName

            _connectionString = connectionString;
            _tableName = tableName;
        }

        public async Task CreateTableIfNotExists()
        {
            var table = GetCloudTable();

            if (await table.ExistsAsync())
            {
                Log.TraceFormat("'{0}' table was not created because it exists.", _tableName);
            }
            else
            {
                Log.TraceFormat("Creating '{0}' table...", _tableName);
                await table.CreateAsync();
                Log.TraceFormat("Created '{0}' table.", _tableName);
            }
        }

        public async Task DeleteTableIfExists()
        {
            var table = GetCloudTable();

            if (await table.ExistsAsync())
            {
                Log.TraceFormat("Deleting '{0}' table...", _tableName);
                await table.DeleteAsync();
                Log.TraceFormat("Deleted '{0}' table.", _tableName);
            }
            else
            {
                Log.TraceFormat("'{0}' table was not deleted because it does not exist.", _tableName);
            }
        }

        private CloudTable GetCloudTable()
        {
            return GetCloudTable(_connectionString, _tableName);
        }

        internal static CloudTable GetCloudTable(string connectionString, string tableName)
        {
            // todo: validate tableName

            var storage = CloudStorageAccount.Parse(connectionString);
            var client = storage.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);

            return table;
        }
    }
}