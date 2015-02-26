using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic.Infrastructure
{
    public class EntityValidator : IEntityValidator
    {
        private readonly IKeyValidator _keyValidator;

        public EntityValidator()
            : this(new KeyValidator())
        {
        }

        public EntityValidator(IKeyValidator keyValidator)
        {
            _keyValidator = keyValidator;
        }

        public void Validate(ITableEntity entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(entity, context, results, /*validateAllProperties*/true);

            _keyValidator.ValidatePartitionKey(entity.PartitionKey, results);
            _keyValidator.ValidateRowKey(entity.RowKey, results);

            if (results.Count == 0)
            {
                return;
            }

            // todo: get validation attribute & value
            var exceptions = results.Select(r => new ValidationException(r, null, null));

            throw new AggregateException(exceptions);
        }
    }
}