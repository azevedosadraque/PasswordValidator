using Domain.ValueObjects;
using MediatR;

namespace Application.Request.ValidatePassword
{
    public class ValidatePasswordRequest(string password) : IRequest<PasswordValidatorResult>
    {
        public string Password { get; private set; } = password;
    }
}
