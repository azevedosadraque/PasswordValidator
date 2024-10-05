using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using TechTalk.SpecFlow;
using WebApi;
using WebApi.Dto;
using WebApi.Responses;

namespace AcceptanceTest.Steps
{

    [Binding]
    public class PasswordValidationSteps
    {
        private readonly HttpClient _client;
        private HttpResponseMessage? _response;
        private ValidatePasswordDto? _passwordDto;

        public PasswordValidationSteps()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [Given(@"I have the password ""(.*)""")]
        public void GivenIHaveThePassword(string password)
        {
            _passwordDto = new ValidatePasswordDto(password);
        }

        [When(@"I submit the password for validation")]
        public async Task WhenISubmitThePasswordForValidation()
        {
            _response = await _client.PostAsJsonAsync("api/password/validate", _passwordDto);
        }

        [Then(@"the validation result should be ""(.*)""")]
        public async Task ThenTheValidationResultShouldBe(string expectedResult)
        {
            var apiResponse = await _response!.Content.ReadFromJsonAsync<ApiResponseSuccess<PasswordValidatorResult>>();

            bool expectedIsValid = expectedResult == "valid";
            apiResponse!.Data!.IsValid.Should().Be(expectedIsValid);
        }
    }
}