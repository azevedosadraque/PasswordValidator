using Application.Request.ValidatePassword;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Dto;
using WebApi.Responses;

namespace UnitTests.WebApi.Controllers
{
    public class PasswordControllerTest
    {
        private readonly PasswordController _controller;
        private readonly Mock<IMediator> _mediatorMock;

        public PasswordControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PasswordController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Validate_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            //Arrange
            _controller.ModelState.AddModelError("Password", "The password fiel is required");

            var validatePasswordDto = new ValidatePasswordDto("");

            //Act
            var result = await _controller.Validate(validatePasswordDto);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponseError>(badRequestResult.Value);

            Assert.Equal("Validation failed", apiResponse.Message);
            Assert.Contains("The password fiel is required", apiResponse.Errors!);
        }

        [Fact]
        public async Task Validate_ShouldReturnOk_WhenPasswordISValid()
        {
            //Arrange
            var validatePassowordDto = new ValidatePasswordDto("AbTp9!fok");
            var passwordValidatorResult = new PasswordValidatorResult(true, []);

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<ValidatePasswordRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(passwordValidatorResult);

            //Act
            var result = await _controller .Validate(validatePassowordDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponseSuccess<PasswordValidatorResult>>(okResult.Value);

            Assert.True(apiResponse.Data!.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldCallMediatorSend_WithCorrectRequest()
        {
            //Arrange
            var validatePassowordDto = new ValidatePasswordDto("AbTp9!fok");
            var passwordValidatorResult = new PasswordValidatorResult(true, []);

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<ValidatePasswordRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(passwordValidatorResult);

            //Act
            var result = await _controller.Validate(validatePassowordDto);

            //Assert
            _mediatorMock.Verify(mediator => mediator.Send(It.Is<ValidatePasswordRequest>(req =>
                req.Password == validatePassowordDto.Password), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Validate_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var validatePasswordDto = new ValidatePasswordDto("ValidPassword123!");

            // Simula uma exceção ao chamar o Mediator
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ValidatePasswordRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _controller.Validate(validatePasswordDto);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred", internalServerErrorResult.Value);
        }
    }
}