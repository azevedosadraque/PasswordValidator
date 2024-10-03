using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Request.ValidatePassword
{
    public class ValidatePasswordRequestHandler : IRequestHandler<ValidatePasswordRequest, bool>
    {
        private readonly IPasswordValidator _passwordValidator;

        public ValidatePasswordRequestHandler(IPasswordValidator passwordValidator)
        {
            _passwordValidator = passwordValidator;
        }

        public Task<bool> Handle(ValidatePasswordRequest request, CancellationToken cancellationToken)
        {
            var password = new Password(request.Password);
            var isValid = _passwordValidator.IsValid(password.Value);

            return Task.FromResult(isValid);
        }
    }
}
