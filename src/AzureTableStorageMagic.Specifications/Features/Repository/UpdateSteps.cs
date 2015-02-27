using System;
using System.Linq;
using AzureTableStorageMagic.Infrastructure;
using AzureTableStorageMagic.Specifications.Support;
using FakeItEasy;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Features.Repository
{
    [Binding]
    public class UpdateSteps
    {
        private readonly ActualData _actual;
        private readonly IAzureStorageEmulator _storageEmulator;
        private readonly GivenData _given;
        private readonly IHttpStatusCodeValidator _httpStatusCodeValidator;
        private readonly Lazy<Repository<DummyTableEntity>> _repository;
        private string _originalFirstName;

        public UpdateSteps(IAzureStorageEmulator storageEmulator, GivenData given, ActualData actual)
        {
            _storageEmulator = storageEmulator;
            _given = given;
            _actual = actual;

            _given.EntityValidator = A.Fake<IEntityValidator>();
            _httpStatusCodeValidator = A.Fake<IHttpStatusCodeValidator>();
            _repository = new Lazy<Repository<DummyTableEntity>>(() => new Repository<DummyTableEntity>(_given.ConnectionString, _given.TableName, _given.EntityValidator, _httpStatusCodeValidator));

            A.CallTo(() => _httpStatusCodeValidator.IsOK(A<int>.That.Matches(i => i < 400))).Returns(true);
        }

        [Given(@"entity exists")]
        public void GivenEntityExists()
        {
            _repository.Value.AddEntity(_given.DummyEntity).Wait();
            _originalFirstName = _given.DummyEntity.FirstName;
        }

        [Given(@"entity has changed")]
        public void GivenEntityHasChanged()
        {
            _given.DummyEntity.FirstName = "Changed First Name";
        }

        [Given(@"entity does not exist")]
        public void GivenEntityDoesNotExist()
        {
            _given.CloudTable.Execute(TableOperation.Delete(_given.DummyEntity));
        }

        [Given(@"Update\(entity\) result\.HttpStatusCode is 400 or above")]
        public void GivenUpdateEntityResult_HttpStatusCodeIs400OrAbove()
        {
            A.CallTo(() => _httpStatusCodeValidator.IsOK(A<int>.Ignored)).Returns(false);
        }

        [When(@"Update\(entity\) is called")]
        public void WhenUpdateEntityIsCalled()
        {
            _actual.ExecuteWhen(() => _repository.Value.UpdateEntity(_given.DummyEntity));
        }

        [Then(@"the table should not be updated")]
        public void ThenTheTableShouldNotBeUpdated()
        {
            _storageEmulator.StartEmulatorIfNotRunning(); // Required for 'when Azure service is not available' scenario.

            var query = new TableQuery<DummyTableEntity>();
            var rows = _given.CloudTable.ExecuteQuery(query);
            var row = rows.Single();

            row.FirstName.Should().Be(_originalFirstName);
        }
    }
}