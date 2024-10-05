using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WebApi;
using WebApi.Dto;
using WebApi.Responses;

namespace IntegrationTest.WebApi
{
    public class PasswordControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PasswordControllerTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("AbTp9!fok", HttpStatusCode.OK, true)]     // Valid password, meets all criteria
        [InlineData("aa", HttpStatusCode.OK, false)]           // Invalid: too short
        [InlineData("AbTp9fok", HttpStatusCode.OK, false)]     // Invalid: missing special character
        [InlineData("", HttpStatusCode.OK, false)]             // Invalid: empty password
        [InlineData("AbTp9!foo", HttpStatusCode.OK, false)]    // Invalid: repeated characters
        [InlineData("AbTp9!foA", HttpStatusCode.OK, false)]    // Invalid: repeated characters ('A' uppercase at the beginning and end)
        [InlineData("AbTp9!foa", HttpStatusCode.OK, false)]    // Invalid: repeated characters ('A' uppercase at the beginning and 'a' lowercase at the end)
        [InlineData("AbTp9 fok", HttpStatusCode.OK, false)]    // Invalid: contains spaces
        [InlineData("AbTp9@fok", HttpStatusCode.OK, true)]     // Valid password with special character '@'
        [InlineData("abcdEFGH1@", HttpStatusCode.OK, true)]    // Valid: minimum 9 characters, includes uppercase, lowercase, number, special character
        [InlineData("AbTp!2k3L@", HttpStatusCode.OK, true)]    // Valid password with all required elements
        [InlineData("123456789", HttpStatusCode.OK, false)]    // Invalid: only numbers, no letters or special characters
        [InlineData("ABCDEFGHI", HttpStatusCode.OK, false)]    // Invalid: only uppercase letters, no numbers or special characters
        [InlineData("abcdefghi", HttpStatusCode.OK, false)]    // Invalid: only lowercase letters, no numbers or special characters
        [InlineData("!@#$%^&*()-+", HttpStatusCode.OK, false)] // Invalid: only special characters, no letters or numbers

        public async Task ValidatePassword_WithVariousInputs_ReturnsExpectedResults(string password, HttpStatusCode expectedStatusCode, bool expectedIsValid)
        {
            // Arrange
            var validatePasswordDto = new ValidatePasswordDto(password);

            // Act
            var response = await _client.PostAsJsonAsync("/api/password/validate", validatePasswordDto);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseSuccess<PasswordValidatorResult>>();

                apiResponse.Should().NotBeNull();
                apiResponse!.Data!.IsValid.Should().Be(expectedIsValid);
            }
        }
    }
}