using Domain.Interfaces;

namespace Domain.Services
{
    public class PasswordValidator : IPasswordValidator
    {
        private readonly string _allowedSpecialChars = "!@#$%^&*()-+";

        public bool IsValid(string password)
        {
            return Validate(password).Count == 0;
        }

        public List<string> Validate(string password)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(password) || password.Length < 8)
                errors.Add("A senha deve ter no mínimo 8 caracteres.");

            if (!password.Any(char.IsUpper))
                errors.Add(".");

            if (!password.Any(char.IsLower))
                errors.Add("A senha deve conter pelo menos uma letra minúscula.");

            if (!password.Any(char.IsDigit))
                errors.Add("A senha deve conter pelo menos um número.");

            if (!password.Any(ch => _allowedSpecialChars.Contains(ch)))
                errors.Add($"A senha deve conter um pelo menos um dos seguintes caracteres especiais: {_allowedSpecialChars}");

            if (password.Contains(" "))
                errors.Add("A senha não pode conter espaços em branco");

            if (password.Distinct().Count() != password.Length)
                errors.Add("A senha não pode conter caracteres repetidos.");

            if (!password.All(ch => char.IsLetterOrDigit(ch) || _allowedSpecialChars.Contains(ch)))
                errors.Add($"A senha contém caracteres inválidos. Apenas letras, números e os caracteres especiais {_allowedSpecialChars} são permitidos");

            return errors;
        }
    }
}
