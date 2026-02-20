using Microsoft.AspNetCore.Mvc;
using Web.Services;
using Web.Models;

namespace Web.Controllers.API
{
    [ApiController]
    [Route("api/error")]
    public class ErrorApiController : ControllerBase
    {
        private readonly ErrorAnalysisService _service;
        private readonly AiDecisionService _aiDecision;
        private readonly OpenAiService _openAi;
        private readonly ErrorHistoryService _historyService;
        private readonly CodeFixService _codeFix;

        public ErrorApiController(ErrorHistoryService historyService)
        {
            _service = new ErrorAnalysisService();
            _aiDecision = new AiDecisionService();
            _openAi = new OpenAiService();
            _historyService = historyService;
            _codeFix = new CodeFixService();

        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] ErrorAnalysisRequest request)
        {
            var result = _service.Analyze(request.ErrorMessage);

            bool needsAi = _aiDecision.ShouldUseAI(result.confidence);

            string explanation;

            if (request.ExplanationMode == "beginner")
            {
                explanation = "BEGINNER MODE:\n" + result.explanation;
            }
            else
            {
                explanation = "DEVELOPER MODE:\n" + result.explanation;
            }


            string suggestion = result.suggestion;
            /*
                        if (needsAi)
                        {
                            var aiResponse = await _openAi.AnalyzeErrorAsync(
                                request.ErrorMessage + "\nCODE:\n" + request.CodeSnippet,
                                request.ExplanationMode
                            );

                            explanation = aiResponse;
                            suggestion = "AI generated suggestion";
                        }
            */
            if (needsAi)
            {
                explanation = "AI analysis temporarily disabled. Using local analysis.";
                suggestion = "Local engine suggestion";
            }

            // HISTORY ADD
            _historyService.Add(new ErrorHistory
            {
                ErrorMessage = request.ErrorMessage,
                CodeSnippet = request.CodeSnippet,
                Explanation = explanation
            });
            var fixes = _codeFix.GenerateFix(request.ErrorMessage, request.CodeSnippet, request.ExplanationMode);

            string riskLevel;

            if (result.confidence >= 80)
                riskLevel = "Low";
            else if (result.confidence >= 50)
                riskLevel = "Medium";
            else
                riskLevel = "High";


            return Ok(new
            {
                explanation,
                suggestion,
                confidence = result.confidence,
                riskLevel = riskLevel,
                aiRequired = needsAi,
                fixedCode = fixes.primaryFix,
                alternativeFix = fixes.alternativeFix
            });


        }
    }
}
