using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Request.ValidatePassword
{
    public class ValidatePasswordRequestHandler : IRequestHandler<ValidatePasswordRequest, PasswordValidatorResult>
    {
        private readonly IPasswordValidator _passwordValidator;
        private readonly ILogger<ValidatePasswordRequestHandler> _logger;

        public ValidatePasswordRequestHandler(IPasswordValidator passwordValidator, 
                                              ILogger<ValidatePasswordRequestHandler> logger)
        {
            _passwordValidator = passwordValidator;
            _logger = logger;
        }

        public Task<PasswordValidatorResult> Handle(ValidatePasswordRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting password validation for user request at {Time}", DateTime.UtcNow);

            var passwordValidatorResult = _passwordValidator.Validate(request.Password);

            LogginValidationResult(passwordValidatorResult);

            return Task.FromResult(passwordValidatorResult);
        }

        private void LogginValidationResult(PasswordValidatorResult passwordValidatorResult)
        {
            var dateTimeUtcNow = DateTime.UtcNow;

            if (passwordValidatorResult.IsValid)
                _logger.LogWarning("Password validation succeded at {Time}", dateTimeUtcNow);
            else 
                _logger.LogWarning("Password validation failed at {Time}. Errors: {errors}", dateTimeUtcNow, passwordValidatorResult.Errors);
        }
    }
}
