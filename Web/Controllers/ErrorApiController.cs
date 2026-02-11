using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers.API
{
    [ApiController]
    [Route("api/error")]
    public class ErrorApiController : ControllerBase
    {
        private readonly ErrorAnalysisService _service;
        private readonly AiDecisionService _aiDecision;
        private readonly OpenAiService _openAi;

        public ErrorApiController()
        {
            _service = new ErrorAnalysisService();
            _aiDecision = new AiDecisionService();
            _openAi = new OpenAiService();
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] Web.Models.ErrorAnalysisRequest request)

        {
            var result = _service.Analyze(request.ErrorMessage);

            bool needsAi = _aiDecision.ShouldUseAI(result.confidence);

            string explanation = result.explanation;
            string suggestion = result.suggestion;

            if (needsAi)
            {
                var aiResponse = await _openAi.AnalyzeErrorAsync(
                request.ErrorMessage + "\nCODE:\n" + request.CodeSnippet
                );

                explanation = aiResponse;
                suggestion = "AI generated suggestion";
            }

            return Ok(new
            {
                explanation,
                suggestion,
                confidence = result.confidence,
                aiRequired = needsAi
            });
        }



    }
}
