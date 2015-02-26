using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using AzureTableStorageMagic.Infrastructure;
using AzureTableStorageMagic.Specifications.Support;
using FakeItEasy;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Features.Repository
{
    [Binding]
    public class AddSteps
    {
        private readonly IAzureStorageEmulator _storageEmulator;
        private readonly GivenData _given;
        private readonly ActualData _actual;
        private readonly Lazy<Repository<DummyTableEntity>> _repository;
        private readonly IEntityValidator _entityValidator;
        private readonly IHttpStatusCodeValidator _httpStatusCodeValidator;
        private const string FeatureName = "Repository.Add";

        public AddSteps(IAzureStorageEmulator storageEmulator, GivenData given, ActualData actual)
        {
            _storageEmulator = storageEmulator;
            _given = given;
            _actual = actual;

            _entityValidator = A.Fake<IEntityValidator>();
            _httpStatusCodeValidator = A.Fake<IHttpStatusCodeValidator>();
            _repository = new Lazy<Repository<DummyTableEntity>>(() => new Repository<DummyTableEntity>(_given.ConnectionString, _given.TableName, _entityValidator, _httpStatusCodeValidator));

            A.CallTo(() => _httpStatusCodeValidator.IsOK(A<int>.That.Matches(i => i < 400))).Returns(true);

        }

        [Given(@"Windows Azure Storage Emulator is running")]
        public void GivenWindowsAzureStorageEmulatorIsRunning()
        {
            _storageEmulator.StartEmulatorIfNotRunning();
        }

        [Given(@"table exists")]
        public void GivenTableExists()
        {
            new AzureTableStorageMagic.Table(AzureStorageEmulator.ConnectionString, _given.TableName).CreateTableIfNotExists().Wait();
        }

        [Given(@"entity is valid")]
        public void GivenEntityIsValid()
        {
            _given.TableEntity = new DummyTableEntity();
        }

        [Given(@"entity is null")]
        public void GivenEntityIsNull()
        {
            _given.TableEntity = null;
        }

        [Given(@"entity is not valid")]
        public void GivenEntityIsNotValid()
        {
            _given.TableEntity = new DummyTableEntity();

            A.CallTo(() => _entityValidator.Validate(A<ITableEntity>.Ignored)).Invokes(() => { throw new ValidationException(); });
        }

        [Given(@"Windows Azure Storage Emulator is not running")]
        public void GivenWindowsAzureStorageEmulatorIsNotRunning()
        {
            _storageEmulator.StopEmulatorIfIsRunning();
        }

        [Given(@"the table does not exist")]
        public void GivenTheTableDoesNotExist()
        {
            new AzureTableStorageMagic.Table(AzureStorageEmulator.ConnectionString, _given.TableName).DeleteTableIfExists().Wait();
        }

        [Given(@"entity\.RowKey is null")]
        public void GivenEntity_RowKeyIsNull()
        {
            _given.TableEntity.RowKey = null;
        }

        [Given(@"Add\(entity\) result\.HttpStatusCode is 400 or above")]
        public void GivenAddEntityResult_HttpStatusCodeIsOrAbove()
        {
            A.CallTo(() => _httpStatusCodeValidator.IsOK(A<int>.Ignored)).Returns(false);
        }
        
        [When(@"Add\(entity\) is called")]
        public void WhenAddEntityIsCalled()
        {
            _actual.ExecuteWhen(() => _repository.Value.AddEntity((DummyTableEntity)_given.TableEntity));
        }

        [Then(@"entity should be added to the table")]
        public void ThenEntityShouldBeAddedToTheTable()
        {
            GetRowCount().Should().Be(1);
        }

        [Scope(Feature = FeatureName)]
        [Then(@"ValidationException should be thrown for entity")]
        public void ThenValidationExceptionShouldBeThrownForEntity()
        {
            ThenExceptionShouldBeThrown<ValidationException>();
        }

        [Then(@"the entity should not be added to the table")]
        public void ThenTheEntityShouldNotBeAddedToTheTable()
        {
            _storageEmulator.StartEmulatorIfNotRunning();

            GetRowCount().Should().Be(0);
        }

        [Then(@"WebException with '(.*)' message should be thrown")]
        public void ThenWebExceptionWithMessageShouldBeThrown(string expectedMessage)
        {
            ThenExceptionWithMessageShouldBeThrown<WebException>(expectedMessage);
        }

        [Then(@"StorageException with '(.*)' message should be thrown")]
        public void ThenStorageExceptionWithMessageShouldBeThrown(string expectedMessage)
        {
            ThenExceptionWithMessageShouldBeThrown<StorageException>(expectedMessage);
        }

        private void ThenExceptionWithMessageShouldBeThrown<TException>(string expectedMessage) where TException : Exception
        {
            var repositoryException = ThenExceptionShouldBeThrown<TException>();

            repositoryException.Message.Should().Be(expectedMessage);
        }

        private TException ThenExceptionShouldBeThrown<TException>()
        {
            var aggregateException = _actual.Exception.Should().BeOfType<AggregateException>().Which;

            aggregateException.InnerExceptions.Count().Should().Be(1);

            var repositoryException = aggregateException.InnerExceptions.First().Should().BeOfType<RepositoryException>().Which;

            return repositoryException.InnerException.Should().BeOfType<TException>().Which;
        }

        private int GetRowCount()
        {
            var query = new TableQuery<DummyTableEntity>();
            var rows = _given.CloudTable.ExecuteQuery(query);

            return rows.Count();
        }
    }
}
