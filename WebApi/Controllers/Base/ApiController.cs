using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;

namespace WebApi.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {

        protected IActionResult? ValidateModelState()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponseError("Validation failed", errors));
            }

            return null;
        }
    }
}