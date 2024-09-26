using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace banking.Attributes
{
    // Custom attribute class derived from ValidationAttribute for validating email format
    public class IsEmailAttribute : ValidationAttribute
    {
        // Regular expression pattern to match a basic email structure (e.g., user@domain.com)
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        // Override the base method for custom validation logic
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if the provided value is a string and it's not null or empty
            if (value is string email && !string.IsNullOrEmpty(email))
            {
                // If the email matches the pattern (i.e., it's valid), return success
                if (Regex.IsMatch(email, EmailPattern))
                {
                    return ValidationResult.Success;
                }
                // Otherwise, return an error result with a custom error message
                else
                {
                    return new ValidationResult("Invalid email format");
                }
            }

            return ValidationResult.Success;
        }
    }
}
