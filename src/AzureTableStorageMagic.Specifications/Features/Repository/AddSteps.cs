using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using AzureTableStorageMagic.Infrastructure;
using AzureTableStorageMagic.RepositoryExceptions;
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
        private const string FeatureName = "Repository.Add";
        private readonly ActualData _actual;
        private readonly GivenData _given;
        private readonly IHttpStatusCodeValidator _httpStatusCodeValidator;
        private readonly Lazy<Repository<DummyTableEntity>> _repository;
        private readonly IAzureStorageEmulator _storageEmulator;

        public AddSteps(IAzureStorageEmulator storageEmulator, GivenData given, ActualData actual)
        {
            _storageEmulator = storageEmulator;
            _given = given;
            _actual = actual;

            _given.EntityValidator = A.Fake<IEntityValidator>();
            _httpStatusCodeValidator = A.Fake<IHttpStatusCodeValidator>();
            _repository = new Lazy<Repository<DummyTableEntity>>(() => new Repository<DummyTableEntity>(_given.ConnectionString, _given.TableName, _given.EntityValidator, _httpStatusCodeValidator));

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
            // nothing to do. Given.DummyTable is created with Given ctor.
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

            A.CallTo(() => _given.EntityValidator.Validate(A<ITableEntity>.Ignored)).Invokes(() => { throw new ValidationException(new ValidationResult("fake", new[] { "entity" }), null, null); });
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
            _actual.ExecuteWhen(() => _repository.Value.AddEntity(_given.DummyEntity));
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
            _storageEmulator.StartEmulatorIfNotRunning(); // Required for 'when Azure service is not available' scenario.

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

        [Then(@"AggregateException should be thrown")]
        public void ThenAggregateExceptionShouldBeThrown()
        {
            _actual.AggregateException = _actual.Exception.Should().BeOfType<AggregateException>().Which;
        }

        [Then(@"AggregateException\.InnerExceptions should be AddEntityException")]
        public void ThenAggregateException_InnerExceptionsShouldBeAddEntityException()
        {
            _actual.AggregateException.InnerExceptions.Count.Should().Be(1);
            _actual.AddEntityException = _actual.AggregateException.InnerException.Should().BeOfType<AddEntityException>().Which;
        }

        [Then(@"AddEntityException\.InnerException should be ValidationException")]
        public void ThenAddEntityException_InnerExceptionShouldBeValidationException()
        {
            _actual.AddEntityException.InnerException.Should().BeOfType<ValidationException>();
        }

        [Then(@"AggregateException\.InnerExceptions should be ArgumentNullException")]
        public void ThenAggregateException_InnerExceptionsShouldBeArgumentNullException()
        {
            _actual.AggregateException.InnerExceptions.Count.Should().Be(1);
            _actual.AggregateException.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [Then(@"AddEntityException\.InnerException should be ServerNotFoundException")]
        public void ThenAddEntityException_InnerExceptionShouldBeServerNotFoundException()
        {
            _actual.ServerNotFoundException = _actual.AddEntityException.InnerException.Should().BeOfType<ServerNotFoundException>().Which;
        }

        [Then(@"ServerNotFound\.InnerException should be StorageException with '(.*)' message")]
        public void ThenServerNotFound_InnerExceptionShouldBeStorageExceptionWithMessage(string expectedMessage)
        {
            _actual.ServerNotFoundException.InnerException.Should()
                .BeOfType<StorageException>().Which
                .Message.Should().Be(expectedMessage);
        }

        [Then(@"AddEntityException\.InnerException should be TableNotFoundException")]
        public void ThenAddEntityException_InnerExceptionShouldBeTableNotFoundException()
        {
            _actual.TableNotFoundException = _actual.AddEntityException.InnerException.Should().BeOfType<TableNotFoundException>().Which;
        }

        [Then(@"TableNotFound\.InnerException should be StorageException with '(.*)' message")]
        public void ThenTableNotFound_InnerExceptionShouldBeStorageExceptionWithMessage(string expectedMessage)
        {
            _actual.TableNotFoundException.InnerException.Should()
                .BeOfType<StorageException>().Which
                .Message.Should().Be(expectedMessage);
        }

        [Then(@"AddEntityException\.InnerException should be WebException with '(.*)' message")]
        public void ThenAddEntityException_InnerExceptionShouldBeWebExceptionWithNoContentTThrowTheErrorItself_Message(string expectedMessage)
        {
            _actual.AddEntityException.InnerException.Should()
                .BeOfType<WebException>().Which
                .Message.Should().Be(expectedMessage);
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