using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AzureTableStorageMagic.RepositoryExceptions;
using AzureTableStorageMagic.Specifications.Support;
using Common.Logging;
using FluentAssertions;
using TechTalk.SpecFlow;
using SUT = AzureTableStorageMagic.Infrastructure;

namespace AzureTableStorageMagic.Specifications.Features.Infrastructure.EntityValidator
{
    [Binding]
    public class ValidateSteps
    {
        private static readonly ILog Log = LogManager.GetLogger<ValidateSteps>();

        private readonly SUT.EntityValidator _validator;
        private readonly GivenData _given;
        private readonly ActualData _actual;

        public ValidateSteps(SUT.EntityValidator validator, GivenData given, ActualData actual)
        {
            _validator = validator;
            _given = given;
            _actual = actual;
        }

        [When(@"Validate\(entity\) is called")]
        public void WhenValidateEntityIsCalled()
        {
            _actual.ExecuteWhen(() => _validator.Validate(_given.DummyEntity));
        }

        [Then(@"an exception should not be thrown")]
        public void ThenAnExceptionShouldNotBeThrown()
        {
            _actual.Exception.Should().BeNull();
        }

        [Then(@"ValidationException should be thrown for (.*)")]
        public void ThenValidationExceptionShouldBeThrownFor(string expectedPropertyNames)
        {
            Log.Trace("Enter - ThenValidationExceptionShouldBeThrownFor(string expectedPropertyNames)");

            var aggregateException = _actual.Exception.Should().BeOfType<AggregateException>().Which;
            var expectedInvalidPropertyNames = expectedPropertyNames.Trim('\'').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToArray();
            var actualInvalidPropertyNames = GetNamesOfInvalidProperties(aggregateException).ToArray();

            Log.DebugFormat("expectedInvalidPropertyNames: {0}", string.Join(", ", expectedInvalidPropertyNames));
            Log.DebugFormat("actualInvalidPropertyNames: {0}", string.Join(", ", actualInvalidPropertyNames));

            actualInvalidPropertyNames.ShouldAllBeEquivalentTo(expectedInvalidPropertyNames);

            Log.Trace("Exit - ThenValidationExceptionShouldBeThrownFor(string expectedPropertyNames)");
        }

        private static IEnumerable<string> GetNamesOfInvalidProperties(AggregateException aggregateException)
        {
            var validationExceptions = aggregateException.InnerExceptions.Select(GetValidationException).ToArray();
            var memberNames = validationExceptions.SelectMany(e => e.ValidationResult.MemberNames).ToArray();

            return memberNames;
        }

        private static ValidationException GetValidationException(Exception e)
        {
            var validationException = e as ValidationException;

            if (validationException != null)
            {
                return validationException;
            }

            var repositoryException = e as RepositoryException;

            if (repositoryException != null)
            {
                validationException = repositoryException.InnerException as ValidationException;
            }

            if (validationException == null)
            {
                throw new Exception(string.Format("Expected to find ValidationException from '{0}'.", e.GetType()));
            }

            return validationException;
        }
    }
}
