using System.Threading.Tasks;

namespace AzureTableStorageMagic
{
    public interface ITable
    {
        Task DeleteTableIfExists();
        Task CreateTableIfNotExists();
    }
}
