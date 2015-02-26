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
        public TableEntity TableEntity;
        public ICollection<ValidationResult> ValidationResults = new List<ValidationResult>();

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