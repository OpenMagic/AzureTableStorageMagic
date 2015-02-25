using System;
using AzureTableStorageMagic.Specifications.Support;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Features.Repository
{
    [Binding]
    public class AddSteps
    {
        private readonly ITable _table;
        private readonly GivenData _given;

        public AddSteps(ITable table, GivenData given)
        {
            _table = table;
            _given = given;
        }

        [Given(@"Windows Azure Storage Emulator is running")]
        public void GivenWindowsAzureStorageEmulatorIsRunning()
        {
            WindowAzureStorageEmulator.StartEmulator();
            _given.ConnectionString = WindowAzureStorageEmulator.ConnectionString;
        }

        [Given(@"table exists")]
        public void GivenTableExists()
        {
            _table.CreateTableIfNotExists(_given.ConnectionString, _given.TableName).Wait();
        }

        [Given(@"entity is valid")]
        public void GivenEntityIsValid()
        {
            throw new NotImplementedException();
        }

        [Given(@"entity is null")]
        public void GivenEntityIsNull()
        {
            throw new NotImplementedException();
        }

        [Given(@"Windows Azure Storage Emulator is not running")]
        public void GivenWindowsAzureStorageEmulatorIsNotRunning()
        {
            throw new NotImplementedException();
        }

        [Given(@"the table does not exist")]
        public void GivenTheTableDoesNotExist()
        {
            throw new NotImplementedException();
        }

        [When(@"Add\(entity\) is called")]
        public void WhenAddEntityIsCalled()
        {
            throw new NotImplementedException();
        }

        [Then(@"entity should be added to the table")]
        public void ThenEntityShouldBeAddedToTheTable()
        {
            throw new NotImplementedException();
        }

        [Then(@"ArgumentException should be thrown for entity")]
        public void ThenArgumentExceptionShouldBeThrownForEntity()
        {
            throw new NotImplementedException();
        }

        [Then(@"ArgumentNullException should be thrown for entityScenario: when Azure service cannot be reached")]
        public void ThenArgumentNullExceptionShouldBeThrownForEntityScenarioWhenAzureServiceCannotBeReached()
        {
            throw new NotImplementedException();
        }

        [Then(@"IFailedHandler\.Add\(connectionString, tableName, entity\) should be called")]
        public void ThenIFailedHandler_AddConnectionStringTableNameEntityShouldBeCalled()
        {
            throw new NotImplementedException();
        }

        [Then(@"the entity should be added to the table")]
        public void ThenTheEntityShouldBeAddedToTheTable()
        {
            throw new NotImplementedException();
        }
    }
}
