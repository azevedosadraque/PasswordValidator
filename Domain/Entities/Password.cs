namespace Domain.Entities
{
    public class Password
    {
        public string Value { get; private set; }

        public Password(string value)
        {
            Value = value;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Value) || Value.Length < 8)
                return false;

            bool hasUpperCase = Value.Any(char.IsUpper);
            bool hasLowerCase = Value.Any(char.IsLower);
            bool hasDigit = Value.Any(char.IsDigit);
            bool hasSpecialChar = Value.Any(ch => "!@#$%^&*()-+".Contains(ch));
            bool hasNoWhiteSpace = !Value.Contains(" ");
            bool hasNoRepeatedChars = Value.Distinct().Count() == Value.Length;

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar && hasNoWhiteSpace && hasNoRepeatedChars;
        }
    }
}
