using System.Threading.Tasks;

namespace AzureTableStorageMagic
{
    public interface ITable
    {
        Task DeleteTableIfExists(string connectionString, string tableName);
        Task CreateTableIfNotExists(string connectionString, string tableName);
    }
}
