using Domain.Interfaces;

namespace Domain.Entities
{
    public class Password
    {
        public string Value { get; private set; }

        public Password(string value) => Value = value;

        public bool IsValid(IPasswordValidator validator)
        {
            return validator.IsValid(Value);
        }
    }
}
