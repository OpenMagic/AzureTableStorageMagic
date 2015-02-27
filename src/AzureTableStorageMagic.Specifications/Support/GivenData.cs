using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AzureTableStorageMagic.Infrastructure;
using Common.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Support
{
    [Binding]
    public class GivenData
    {
        private static readonly ILog Log = LogManager.GetLogger<GivenData>();
        private string _tableName;
        public string ConnectionString;
        public ICollection<ValidationResult> ValidationResults = new List<ValidationResult>();
        public IEntityValidator EntityValidator;
        private ITableEntity _tableEntity;

        public GivenData()
        {
            ConnectionString = AzureStorageEmulator.ConnectionString;
            TableEntity = new DummyTableEntity();
        }

        public string TableName
        {
            get { return _tableName ?? (_tableName = string.Format("specs{0}", Guid.NewGuid()).Replace("-", "")); }
            set
            {
                CleanupTable();
                _tableName = value;
            }
        }

        public ITableEntity TableEntity
        {
            get { return _tableEntity; }
            set
            {
                if (_tableEntity != value)
                {
                    if (value == null)
                    {
                        Log.Debug("TableEntity changed to null.");
                    }
                    else
                    {
                        Log.DebugFormat("TableEntity changed to {0}, {1}", value.PartitionKey, value.RowKey);
                    }
                }
                _tableEntity = value;
            }
        }

        public CloudTable CloudTable
        {
            get { return Table.GetCloudTable(ConnectionString, TableName); }
        }

        public DummyTableEntity DummyEntity
        {
            get { return (DummyTableEntity)TableEntity; }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            CleanupTable();
        }

        private void CleanupTable()
        {
            if (string.IsNullOrWhiteSpace(_tableName))
            {
                return;
            }

            new Table(ConnectionString, _tableName).DeleteTableIfExists().Wait();
        }

        public IEnumerable<string> GetValidationErrorMessages()
        {
            return ValidationResults.Select(v => v.ErrorMessage);
        }
    }
}