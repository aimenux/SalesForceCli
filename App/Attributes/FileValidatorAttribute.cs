using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace App.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class FileValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is not string strValue || !File.Exists(strValue))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}