using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InfrastructureLibrary.Infrastructure.Validation
{
    public class PasswordValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || value as string == string.Empty)
            {
                return new ValidationResult("Password can't be empty.");
            }

            var textValue = value as string;

            if (textValue.Length <= 7)
            {
                return new ValidationResult("Password must be at least 8 characters long.");
            }

            if (!textValue.Any(char.IsDigit))
            {
                return new ValidationResult("Password must contain at least one digit.");
            }

            if (!textValue.Any(char.IsUpper))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }

            if (!textValue.Any(char.IsLower))
            {
                return new ValidationResult("Password must contain at least one lowercase letter.");
            }

            if (!textValue.Any(c => !char.IsLetterOrDigit(c)))
            {
                return new ValidationResult("Password must contain at least one special character.");
            }

            return ValidationResult.Success;
        }
    }
}
