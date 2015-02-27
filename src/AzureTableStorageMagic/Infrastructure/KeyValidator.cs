using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EmptyStringGuard;
using NullGuard;

namespace AzureTableStorageMagic.Infrastructure
{
    public class KeyValidator : IKeyValidator
    {
        public const int MaximumLength = 1024;

        // Reference: https://msdn.microsoft.com/library/azure/dd179338.aspx

        public static readonly string[] DisallowedCharacters = {"/", @"\", "#", "?"};

        public void ValidatePartitionKey([AllowNull, AllowEmpty] string partitionKey, ICollection<ValidationResult> validationResults)
        {
            ValidateKey(partitionKey, "PartitionKey", /*allowEmptyStrings*/false, validationResults);
        }

        public void ValidateRowKey([AllowNull, AllowEmpty] string rowKey, ICollection<ValidationResult> validationResults)
        {
            ValidateKey(rowKey, "RowKey", /*allowEmptyStrings*/true, validationResults);
        }

        private static void ValidateKey([AllowNull, AllowEmpty] string value, string propertyName, bool allowEmptyStrings, ICollection<ValidationResult> validationResults)
        {
            ValidateRequired(value, propertyName, allowEmptyStrings, validationResults);

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            ValidateAcceptableCharacters(value, propertyName, validationResults);
            ValidateLength(value, propertyName, validationResults);
        }

        private static void ValidateAcceptableCharacters(string value, string propertyName, ICollection<ValidationResult> validationResults)
        {
            if (!DisallowedCharacters.Any(value.Contains) && !value.ToCharArray().Any(Char.IsControl))
            {
                return;
            }

            var errorMessage = string.Format(@"The {0} cannot contain '/, \, #, ?' or control characters from U+0000 to U+001F & U+007F to U+009F.", propertyName);
            validationResults.Add(new ValidationResult(errorMessage, new[] { propertyName }));
        }

        private static void ValidateLength(string value, string propertyName, ICollection<ValidationResult> validationResults)
        {
            if (value.Length <= MaximumLength)
            {
                return;
            }

            var errorMessage = string.Format("The {0} cannot be greater than {1:N0} characters.", propertyName, MaximumLength);
            validationResults.Add(new ValidationResult(errorMessage, new[] { propertyName }));
        }

        private static void ValidateRequired([AllowNull, AllowEmpty] string value, string propertyName, bool allowEmptyStrings, ICollection<ValidationResult> validationResults)
        {
            var validator = new RequiredAttribute { AllowEmptyStrings = allowEmptyStrings };

            if (allowEmptyStrings && IsWhiteSpace(value))
            {
                // Only whitespace (i.e. not null or empty) is not permitted.
                validator.AllowEmptyStrings = false;
            }

            if (validator.IsValid(value))
            {
                return;
            }

            validationResults.Add(new ValidationResult(validator.FormatErrorMessage(propertyName), new[] { propertyName }));
        }

        private static bool IsWhiteSpace(string value)
        {
            return !string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value);
        }
    }
}