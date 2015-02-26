using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AzureTableStorageMagic.Infrastructure
{
    public interface IKeyValidator
    {
        void ValidatePartitionKey(string partitionKey, ICollection<ValidationResult> validationResults);
        void ValidateRowKey(string rowKey, ICollection<ValidationResult> validationResults);
    }
}