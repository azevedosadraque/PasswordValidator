﻿using Application.Request.ValidatePassword;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Controllers.Base;
using WebApi.Dto;
using WebApi.Responses;

namespace WebApi.Controllers
{
    public class PasswordController : ApiController
    {
        private readonly IMediator _mediator;

        public PasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Validates the provided password", Description = "Receives a password and validates if it meets security criteria.")]
        public async Task<IActionResult> Validate([FromBody] ValidatePasswordDto validatePassword)
        {
            try
            {
                var validationParametersResult = ValidateModelState();
                if (validationParametersResult is not null) return validationParametersResult;

                var request = new ValidatePasswordRequest(validatePassword.Password);
                var result = await _mediator.Send(request);

                return Ok(new ApiResponseSuccess<PasswordValidatorResult>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
