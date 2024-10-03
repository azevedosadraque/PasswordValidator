using Application.Request.ValidatePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Validate([FromBody] string password)
        {
            if (string.IsNullOrEmpty(password))
                return BadRequest("Password cannot be null or empty.");

            var request = new ValidatePasswordRequest(password);
            var isValid = await _mediator.Send(request);

            return Ok(isValid);
        }
    }
}
