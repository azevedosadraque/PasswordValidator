

using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface IPasswordValidator
    {
        PasswordValidatorResult Validate(string password);
    }
}
