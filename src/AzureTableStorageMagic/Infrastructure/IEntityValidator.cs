using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic.Infrastructure
{
    public interface IEntityValidator
    {
        void Validate(ITableEntity entity);
    }
}