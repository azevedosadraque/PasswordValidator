using Domain.Common.Dto;
using Domain.Interfaces;

namespace Domain.Services
{
    public class PasswordValidator : IPasswordValidator
    {
        private readonly string _allowedSpecialChars = "!@#$%^&*()-+";

        public ValidationPasswordDto Validate(string password)
        {
            var errors = Validate(password);

            return new ValidationPasswordDto(errors, errors.Count == 0);
        }

        public List<string> Validate(string password)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(password) || password.Length < 8)
                errors.Add("The password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                errors.Add("The password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                errors.Add("The password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                errors.Add("The password must contain at least one number.");

            if (!password.Any(ch => _allowedSpecialChars.Contains(ch)))
                errors.Add($"The password must contain at least one of the following special characters: {_allowedSpecialChars}");

            if (password.Contains(" "))
                errors.Add("The password cannot contain spaces.");

            if (password.Distinct().Count() != password.Length)
                errors.Add("The password cannot contain repeated characters.");

            if (!password.All(ch => char.IsLetterOrDigit(ch) || _allowedSpecialChars.Contains(ch)))
                errors.Add($"The password contains invalid characters. Only letters, numbers, and the special characters {_allowedSpecialChars} are allowed.");

            return errors;
        }
    }
}
