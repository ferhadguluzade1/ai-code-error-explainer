using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.API
{
    [ApiController]
    [Route("api/learning")]
    public class LearningApiController : ControllerBase
    {
        [HttpPost("ask")]
        public IActionResult Ask([FromBody] LearningQuestion request)
        {
            // Placeholder response
            return Ok(new
            {
                reply = "AI mentor will answer here. (Integration step next)"
            });
        }
    }

    public class LearningQuestion
    {
        public string Question { get; set; }
    }
}