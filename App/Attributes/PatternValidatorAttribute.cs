using System;
using System.ComponentModel.DataAnnotations;

namespace App.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class PatternValidatorAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public PatternValidatorAttribute(int min = 1, int max = 100)
        {
            _min = min;
            _max = max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is not string strValue || string.IsNullOrWhiteSpace(strValue) || !IsLengthInRange(strValue))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }

        private bool IsLengthInRange(string input)
        {
            return input.Length >= _min && input.Length <= _max;
        }
    }
}
