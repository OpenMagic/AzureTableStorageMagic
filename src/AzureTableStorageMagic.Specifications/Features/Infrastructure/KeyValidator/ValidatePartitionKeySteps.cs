using AzureTableStorageMagic.Specifications.Support;
using FluentAssertions;
using TechTalk.SpecFlow;
using SUT = AzureTableStorageMagic.Infrastructure;

namespace AzureTableStorageMagic.Specifications.Features.Infrastructure.KeyValidator
{
    [Binding]
    public class ValidatePartitionKeySteps
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;
        private readonly SUT.KeyValidator _validator;

        public ValidatePartitionKeySteps(SUT.KeyValidator validator, GivenData given, ActualData actual)
        {
            _validator = validator;
            _given = given;
            _actual = actual;
        }

        [When(@"ValidatePartitionKey\(partitionKey, validationResults\) is called")]
        public void WhenValidatePartitionKeyPartitionKeyValidationResultsIsCalled()
        {
            _actual.ExecuteWhen(() => _validator.ValidatePartitionKey(_given.DummyEntity.PartitionKey, _given.ValidationResults));
        }

        [Then(@"validationResults count should be (.*)")]
        public void ThenValidationResultsCountShouldBe(int expectedCount)
        {
            _given.ValidationResults.Count.Should().Be(expectedCount);
        }

        [Then(@"validationResults should include required validation exception for (.*)")]
        public void ThenValidationResultsShouldIncludeRequiredValidationExceptionFor(string keyName)
        {
            var expectedErrorMessage = string.Format("The {0} field is required.", keyName);
            
            _given.GetValidationErrorMessages().Should().Contain(expectedErrorMessage);
        }

        [Then(@"validationResults should include unacceptable character exception for (.*)")]
        public void ThenValidationResultsShouldIncludeUnacceptableCharacterExceptionFor(string keyName)
        {
            var expectedErrorMessage = string.Format(@"The {0} cannot contain '/, \, #, ?' or control characters from U+0000 to U+001F & U+007F to U+009F.", keyName);

            _given.GetValidationErrorMessages().Should().Contain(expectedErrorMessage);
        }

        [Then(@"validationResults should include to maximum length exception for (.*)")]
        public void ThenValidationResultsShouldIncludeToMaximumLengthExceptionFor(string keyName)
        {
            var expectedErrorMessage = string.Format("The {0} cannot be greater than 1,024 characters.", keyName);

            _given.GetValidationErrorMessages().Should().Contain(expectedErrorMessage);
        }
    }
}