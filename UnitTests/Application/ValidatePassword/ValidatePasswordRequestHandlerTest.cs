using Application.Request.ValidatePassword;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Application.ValidatePassword
{
    public class ValidatePasswordRequestHandlerTest
    {
        private readonly Mock<IPasswordValidator> _passwordValidatorMock;
        private readonly Mock<ILogger<ValidatePasswordRequestHandler>> _loggerMock;
        private readonly ValidatePasswordRequestHandler _handler;

        public ValidatePasswordRequestHandlerTest()
        {
            _passwordValidatorMock = new Mock<IPasswordValidator>();
            _loggerMock = new Mock<ILogger<ValidatePasswordRequestHandler>>();

            _handler = new ValidatePasswordRequestHandler(_passwordValidatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidResult_WhenPasswordIsValid()
        {
            // Arrange
            var validPassword = "AbTp9!fok";
            var request = new ValidatePasswordRequest(validPassword);
            var passwordValidatorResult = new PasswordValidatorResult(true, []);

            _passwordValidatorMock
                .Setup(pv => pv.Validate(validPassword))
                .Returns(passwordValidatorResult);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            _loggerMock.Verify(log => log.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Password validation succeded")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }



        [Fact]
        public async Task Handle_ShouldReturnInvalidResult_WhenPasswordIsInvalid()
        {
            // Arrange
            var invalidPassword = "InvalidPassword";
            var request = new ValidatePasswordRequest(invalidPassword);
            var passwordValidatorResult = new PasswordValidatorResult(false, ["The password must be at least 8 characters long", "The password must contain at least one uppercase letter"]);

            _passwordValidatorMock
                .Setup(passwordValidator => passwordValidator.Validate(invalidPassword))
                .Returns(passwordValidatorResult);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password must be at least 8 characters long", result.Errors);
            Assert.Contains("The password must contain at least one uppercase letter", result.Errors);

            _loggerMock.Verify(log => log.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Password validation failed")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallPasswordValidator_Once()
        {
            // Arrange
            var password = "Password123!";
            var request = new ValidatePasswordRequest(password);
            var passwordValidatorResult = new PasswordValidatorResult(true, []);

            _passwordValidatorMock
                .Setup(passwordValidator => passwordValidator.Validate(password))
                .Returns(passwordValidatorResult);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _passwordValidatorMock.Verify(pv => pv.Validate(password), Times.Once);
        }
    }
}
