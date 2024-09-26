using System;
using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

namespace banking.Attributes
{
    /// Custom attribute class derived from ValidationAttribute for validating phone numbers 
    /// using Google's libphonenumber library.
    public class PhoneNumberAttribute : ValidationAttribute
    {
        // Protected method to override the default validation behavior
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if the provided value is a string and it's not null or empty
            if (value is string mobileNumber && !string.IsNullOrEmpty(mobileNumber))
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance(); // Initialize Google's libphonenumber library

                try
                {
                    var phoneNumber = phoneNumberUtil.Parse(mobileNumber, null); // Parse the provided string as a phone number
                    if (phoneNumberUtil.IsValidNumber(phoneNumber)) // Check if the parsed phone number is valid
                    {
                        return ValidationResult.Success; // Return success if the phone number is valid
                    }
                }
                catch (NumberParseException)
                {
                    // Handle parse exceptions if needed (e.g., log or provide a custom error message for invalid formats)
                }
            }

            // Return an error result with a default error message for any other case
            return new ValidationResult("Invalid phone number"); 


        }
    }
}
