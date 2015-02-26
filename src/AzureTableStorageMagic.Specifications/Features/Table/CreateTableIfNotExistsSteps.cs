using AzureTableStorageMagic.Infrastructure;
using AzureTableStorageMagic.Specifications.Support;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Features.Table
{
    [Binding]
    public class CreateTableIfNotExistsSteps
    {
        private readonly AzureTableStorageMagic.Table _table;
        private readonly GivenData _given;
        private readonly ActualData _actual;

        public CreateTableIfNotExistsSteps(GivenData given, ActualData actual)
        {
            _given = given;
            _actual = actual;

            _table = new AzureTableStorageMagic.Table(AzureStorageEmulator.ConnectionString, _given.TableName);
        }

        [Given(@"table does not exist")]
        public void GivenTableDoesNotExist()
        {
            _given.CloudTable.DeleteIfExists();
        }

        [Given(@"connectionString is null")]
        public void GivenConnectionStringIsNull()
        {
            _given.ConnectionString = null;
        }

        [Given(@"connectionString is empty")]
        public void GivenConnectionStringIsEmpty()
        {
            _given.ConnectionString = "";
        }

        [Given(@"table does exist")]
        public void GivenTableDoesExist()
        {
            _given.CloudTable.Create();
        }

        [When(@"CreateTableIfNotExists\(\) is called")]
        public void When_CreateTableIfNotExists_IsCalled()
        {
            _actual.ExecuteWhen(() => _table.CreateTableIfNotExists());
        }

        [Then(@"table should be created")]
        public void ThenTableShouldBeCreated()
        {
            _given.CloudTable.Exists().Should().BeTrue();
        }

        [Then(@"nothing should happen")]
        public void ThenNothingShouldHappen()
        {
            // nothing to do
        }
    }
}