using AzureTableStorageMagic.Infrastructure;
using AzureTableStorageMagic.Specifications.Support;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Features.Table
{
    [Binding]
    public class DeleteTableIfExistsSteps
    {
        private readonly GivenData _given;
        private readonly AzureTableStorageMagic.Table _table;

        public DeleteTableIfExistsSteps(GivenData given)
        {
            _given = given;

            _table = new AzureTableStorageMagic.Table(AzureStorageEmulator.ConnectionString, _given.TableName);
        }

        [When(@"DeleteTableIfExists\(\) is called")]
        public void WhenDeleteTableIfExistsIsCalled()
        {
            _table.DeleteTableIfExists().Wait();
        }

        [Then(@"table should be deleted")]
        public void ThenTableShouldBeDeleted()
        {
            _given.CloudTable.Exists().Should().BeFalse();
        }
    }
}
