using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace banking.Attributes;

public class IsEmailAttribute : ValidationAttribute
{
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string email && !string.IsNullOrEmpty(email))
        {
            if (Regex.IsMatch(email, EmailPattern))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid email format");
            }
        }

        return ValidationResult.Success;
    }
}