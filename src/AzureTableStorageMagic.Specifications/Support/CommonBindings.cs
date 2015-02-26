using System;
using AzureTableStorageMagic.Infrastructure;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureTableStorageMagic.Specifications.Support
{
    [Binding]
    public class CommonBindings
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;

        public CommonBindings(GivenData given, ActualData actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"FirstName is (.*)")]
        public void GivenFirstNameIs(string value)
        {
            _given.DummyEntity.FirstName = value.AsString();
        }

        [Given(@"LastName is (.*)")]
        public void GivenLastNameIs(string value)
        {
            _given.DummyEntity.LastName = value.AsString();
        }

        [Given(@"PartitionKey is (.*)")]
        public void GivenPartitionKeyIs(string value)
        {
            _given.DummyEntity.PartitionKey = value == "invalid" ? GetInvalidKey() : value.AsString();
        }

        [Given(@"RowKey is (.*)")]
        public void GivenRowKeyIs(string value)
        {
            _given.DummyEntity.RowKey = value == "invalid" ? GetInvalidKey() : value.AsString();
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

        private static string GetInvalidKey()
        {
            return RandomGenerator.Boolean() ? 
                RandomGenerator.Character(KeyValidator.DisallowedCharacters) : 
                "".PadRight(KeyValidator.MaximumLength + 1, 'a');
        }
    }
}