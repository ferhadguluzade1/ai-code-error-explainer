using Microsoft.AspNetCore.Mvc;
using Web.Services;
using Web.Models;
using Web.Data;

namespace Web.Controllers.API
{
    [ApiController]
    [Route("api/error")]
    public class ErrorApiController : ControllerBase
    {
        private readonly ErrorAnalysisService _service;
        private readonly AiDecisionService _aiDecision;
        private readonly ErrorHistoryService _historyService;
        private readonly CodeFixService _codeFix;
        private object fixedCode;
        private object alternativeFix;

        public ErrorApiController(
            ErrorHistoryService historyService)
        {
            _service = new ErrorAnalysisService();
            _aiDecision = new AiDecisionService();
            _historyService = historyService;
            _codeFix = new CodeFixService();
        }


        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ErrorMessage)
      && string.IsNullOrWhiteSpace(request.CodeSnippet))
            {
                return BadRequest(new
                {
                    message = "No input provided."
                });
            }
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

            /*
            if (needsAi)
            {
                var aiResponse = await _openAi.AnalyzeStructuredAsync(
                    request.ErrorMessage + "\nCODE:\n" + request.CodeSnippet,
                    request.ExplanationMode
                );

                if (aiResponse != null)
                {
                    explanation = aiResponse.Explanation + "\n\nRoot Cause:\n" + aiResponse.RootCause;

                    suggestion = aiResponse.BestPractice;

                    return Ok(new
                    {
                        explanation,
                        suggestion,
                        confidence = result.confidence,
                        aiRequired = true,
                        fixedCode = aiResponse.FixedCode,
                        alternativeFix = aiResponse.AlternativeFix
                    });
                }
            }
            */

            // HISTORY ADD
            
            _historyService.Add(new ErrorHistory
            {
                ErrorMessage = request.ErrorMessage,
                CodeSnippet = request.CodeSnippet,
                Explanation = explanation
            });

            var fixes = _codeFix.GenerateFix(
                request.ErrorMessage,
                request.CodeSnippet,
                request.ExplanationMode
            );

            string riskLevel;

            if (result.confidence >= 80)
                riskLevel = "Low";
            else if (result.confidence >= 50)
                riskLevel = "Medium";
            else
                riskLevel = "High";

            return Ok(new
            {
                riskLevel = "LOW",
                confidence = 90,
                explanation = explanation,
                suggestion = suggestion,
                fixedCode = fixedCode,
                alternativeFix = alternativeFix,
                aiRequired = false
            });
        }
    }
}