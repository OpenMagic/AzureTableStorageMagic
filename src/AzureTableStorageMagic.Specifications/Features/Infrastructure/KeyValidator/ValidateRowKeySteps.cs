using AzureTableStorageMagic.Specifications.Support;
using TechTalk.SpecFlow;
using SUT = AzureTableStorageMagic.Infrastructure;

namespace AzureTableStorageMagic.Specifications.Features.Infrastructure.KeyValidator
{
    [Binding]
    public class ValidateRowKeySteps
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;
        private readonly SUT.KeyValidator _validator;

        public ValidateRowKeySteps(SUT.KeyValidator validator, GivenData given, ActualData actual)
        {
            _validator = validator;
            _given = given;
            _actual = actual;
        }

        [When(@"ValidateRowKey\(RowKey, validationResults\) is called")]
        public void WhenValidateRowKeyRowKeyValidationResultsIsCalled()
        {
            _actual.ExecuteWhen(() => _validator.ValidateRowKey(_given.DummyEntity.RowKey, _given.ValidationResults));
        }
    }
}
