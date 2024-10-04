using Application.Request.ValidatePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Validates the provided password", Description = "Receives a password and validates if it meets security criteria.")]
        public async Task<IActionResult> Validate([FromBody] ValidatePasswordDto validatePassword)
        {
            if (string.IsNullOrEmpty(validatePassword.Password))
                return BadRequest("Password cannot be null or empty.");

            var request = new ValidatePasswordRequest(validatePassword.Password);
            var isValid = await _mediator.Send(request);

            return Ok(isValid);
        }
    }
}
