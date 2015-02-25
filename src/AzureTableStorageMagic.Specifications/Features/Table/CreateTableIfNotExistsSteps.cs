using System;
using AzureTableStorageMagic.Specifications.Support;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Features.Table
{
    [Binding]
    public class CreateTableIfNotExistsSteps
    {
        private readonly AzureTableStorageMagic.Table _table;
        private readonly GivenData _given;
        private readonly ActualData _actual;

        public CreateTableIfNotExistsSteps(AzureTableStorageMagic.Table table, GivenData given, ActualData actual)
        {
            _table = table;
            _given = given;
            _actual = actual;
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

        [Given(@"tableName is null")]
        public void GivenTableNameIsNull()
        {
            _given.TableName = null;
        }

        [Given(@"tableName is empty")]
        public void GivenTableNameIsEmpty()
        {
            _given.TableName = "";
        }

        [Given(@"table does exist")]
        public void GivenTableDoesExist()
        {
            _given.CloudTable.Create();
        }

        [When(@"CreateTableIfNotExists\(connectionString, tableName\) is called")]
        public void WhenCreateTableIfNotExists_connectionString_tableName_IsCalled()
        {
            _actual.ExecuteWhen(() => _table.CreateTableIfNotExists(_given.ConnectionString, _given.TableName));
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

        [Then(@"ArgumentNullException should be thrown for (.*)")]
        public void ThenArgumentNullExceptionShouldBeThrownFor(string paramName)
        {
            _actual.Exception.Should().BeOfType<ArgumentNullException>();
            _actual.Exception.Message.Should().Be(string.Format("[NullGuard] {0} is null.\r\nParameter name: {0}", paramName));
        }

        [Then(@"ArgumentEmptyStringException should be thrown for (.*)")]
        public void ThenArgumentEmptyStringExceptionShouldBeThrownFor(string paramName)
        {
            _actual.Exception.Should().BeOfType<ArgumentException>();
            _actual.Exception.Message.Should().Be(string.Format("[EmptyStringGuard] {0} is an empty string.\r\nParameter name: {0}", paramName));
        }
    }
}