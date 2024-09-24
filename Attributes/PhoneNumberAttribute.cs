using System;
using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

namespace banking.Attributes;

public class PhoneNumberAttribute : ValidationAttribute
{

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string mobileNumber && !string.IsNullOrEmpty(mobileNumber))
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(mobileNumber, null);
                if (phoneNumberUtil.IsValidNumber(phoneNumber))
                {
                    return ValidationResult.Success;
                }
            }
            catch (NumberParseException)
            {
                // Handle parse exceptions if needed
            }
        }

        return new ValidationResult("Invalid phone number");
    }

}
