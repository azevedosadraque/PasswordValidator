﻿using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class PasswordValidator : IPasswordValidator
    {
        private readonly string _allowedSpecialChars = "!@#$%^&*()-+";

        public PasswordValidatorResult Validate(string password)
        {
            var errors = GetErrors(password);

            return new PasswordValidatorResult(errors.Count == 0, errors);
        }

        private List<string> GetErrors(string password)
        {
            List<string> errors = [];

            if (password.Length < 8)
                errors.Add("The password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                errors.Add("The password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                errors.Add("The password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                errors.Add("The password must contain at least one number.");

            if (!password.Any(ch => _allowedSpecialChars.Contains(ch)))
                errors.Add($"The password must contain at least one of the following special characters: {_allowedSpecialChars}");

            if (password.Contains(' '))
                errors.Add("The password cannot contain spaces.");

            if (password.ToUpper().Distinct().Count() != password.Length)
                errors.Add("The password cannot contain repeated characters.");

            if (!password.All(ch => char.IsLetterOrDigit(ch) || _allowedSpecialChars.Contains(ch)))
                errors.Add($"The password contains invalid characters. Only letters, numbers, and the special characters {_allowedSpecialChars} are allowed.");

            return errors;
        }
    }
}
