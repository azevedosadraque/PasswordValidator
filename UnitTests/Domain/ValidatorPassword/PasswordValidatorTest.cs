using Domain.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnitTests.Domain.ValidatorPassword
{
    public class PasswordValidatorTest
    {
        private readonly PasswordValidator _validator;

        public PasswordValidatorTest()
        {
            _validator = new PasswordValidator();
        }

        [Fact]
        public void Validate_ShouldReturnValid_WhenPasswordIsValid()
        {
            // Arrange
            var validPassword = "AbTp9!fok";

            // Act
            var result = _validator.Validate(validPassword);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordIsTooShort()
        {
            // Arrange
            var shortPassword = "Short1!";

            // Act
            var result = _validator.Validate(shortPassword);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password must be at least 8 characters long.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordHasNoUppercaseLetter()
        {
            // Arrange
            var password = "a1!lower";

            // Act
            var result = _validator.Validate(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password must contain at least one uppercase letter.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordHasNoLowercaseLetter()
        {
            // Arrange
            var password = "1!ABCDEFG";

            // Act
            var result = _validator.Validate(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password must contain at least one lowercase letter.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordHasNoNumber()
        {
            // Arrange
            var password = "AbCdEfG!";

            // Act
            var result = _validator.Validate(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password must contain at least one number.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordHasNoSpecialCharacter()
        {
            // Arrange
            var password = "NoSpecial1";

            // Act
            var result = _validator.Validate(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password must contain at least one of the following special characters: !@#$%^&*()-+", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordContainsSpaces()
        {
            // Arrange
            var passwordWithSpaces = "AbTp 9!fok";

            // Act
            var result = _validator.Validate(passwordWithSpaces);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password cannot contain spaces.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordContainsRepeatedCharacters()
        {
            // Arrange
            var passwordWithRepeatedChars = "AAbTp9!fok";

            // Act
            var result = _validator.Validate(passwordWithRepeatedChars);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password cannot contain repeated characters.", result.Errors);
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordContainsInvalidCharacters()
        {
            // Arrange
            var passwordWithInvalidChars = "AbTp9!fok;";

            // Act
            var result = _validator.Validate(passwordWithInvalidChars);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The password contains invalid characters. Only letters, numbers, and the special characters !@#$%^&*()-+ are allowed.", result.Errors);
        }
    }
}