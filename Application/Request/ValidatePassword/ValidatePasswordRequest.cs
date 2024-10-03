using MediatR;

namespace Application.Request.ValidatePassword
{
    public class ValidatePasswordRequest : IRequest<bool>
    {
        public string Password { get; private set; }

        public ValidatePasswordRequest(string password)
        {
            Password = password;
        }
    }
}
