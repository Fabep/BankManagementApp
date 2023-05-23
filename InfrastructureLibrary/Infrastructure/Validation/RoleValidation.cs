using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLibrary.Infrastructure.Validation
{
    public class RoleValidation : ValidationAttribute
    {
        public RoleValidation() : base("Invalid role.")
        {
            
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Role can't be empty.");
            var textValue = value as string;
            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return (textValue.ToLower() == "admin") || (textValue.ToLower() == "cashier") 
                ? ValidationResult.Success 
                : new ValidationResult(errorMessage);
                
        }
    }
}
