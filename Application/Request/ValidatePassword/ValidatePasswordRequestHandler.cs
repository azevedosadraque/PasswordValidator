using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Request.ValidatePassword
{
    public class ValidatePasswordRequestHandler : IRequestHandler<ValidatePasswordRequest, bool>
    {
        private readonly IPasswordValidator _passwordValidator;
        private readonly ILogger<ValidatePasswordRequestHandler> _logger;

        public ValidatePasswordRequestHandler(IPasswordValidator passwordValidator, 
                                              ILogger<ValidatePasswordRequestHandler> logger)
        {
            _passwordValidator = passwordValidator;
            _logger = logger;
        }

        public Task<bool> Handle(ValidatePasswordRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting password validation for user request at {Time}", DateTime.UtcNow);

            var passwordValidatorResult = _passwordValidator.Validate(request.Password);

            if (!passwordValidatorResult.IsValid)
            {
                _logger.LogWarning("Password validation failed at {Time}. Errors: {errors}", DateTime.UtcNow, passwordValidatorResult.Errors);
                return Task.FromResult(passwordValidatorResult.IsValid);
            }

            _logger.LogWarning("Password validation succeded at {Time}", DateTime.UtcNow);

            return Task.FromResult(passwordValidatorResult.IsValid);
        }
    }
}
