using Domain.Entities;
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

            var validationResult = _passwordValidator.Validate(request.Password);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Password validation failed at {Time}. Errors: {errors}", DateTime.UtcNow, validationResult.Errors);
                return Task.FromResult(validationResult.IsValid);
            }

            _logger.LogWarning("Password validation succeded at {Time}", DateTime.UtcNow);

            return Task.FromResult(validationResult.IsValid);
        }
    }
}
