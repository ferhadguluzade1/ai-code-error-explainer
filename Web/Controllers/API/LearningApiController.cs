using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers.API
{
    [ApiController]
    [Route("api/learning")]
    public class LearningApiController : ControllerBase
    {
        private readonly ErrorHistoryService _historyService;

        public LearningApiController(ErrorHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpPost("ask")]
        public IActionResult Ask([FromBody] LearningQuestion request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new { reply = "Please enter a valid question." });
            }

            var context = _historyService.BuildLearningContext();

            var reply =
$@"AI Learning Mentor

{context}

Your Question:
{request.Question}

Guidance:
Focus on understanding the root cause rather than memorizing fixes.
Practice debugging similar scenarios to strengthen problem-solving skills.
Try to reproduce similar errors and debug them step by step.";

            return Ok(new
            {
                reply = reply
            });
        }
    }

    public class LearningQuestion
    {
        public string Question { get; set; }
    }
}