using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.API
{
    [ApiController]
    [Route("api/error")]
    public class ErrorApiController : ControllerBase
    {
        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return BadRequest("Error message is empty");

            return Ok(new
            {
                explanation = "This is a placeholder explanation for the error.",
                suggestion = "AI will analyze this in later steps."
            });
        }
    }
}
