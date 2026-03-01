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
        private readonly ErrorHistoryService _historyService;
        private readonly CodeFixService _codeFix;
        private readonly OpenAiService _openAi;

        public ErrorApiController(
            ErrorHistoryService historyService,
            OpenAiService openAi)
        {
            _service = new ErrorAnalysisService();
            _aiDecision = new AiDecisionService();
            _historyService = historyService;
            _codeFix = new CodeFixService();
            _openAi = openAi;
        }

        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            /* ---------- VALIDATION ---------- */

            if (string.IsNullOrWhiteSpace(request.ErrorMessage)
                && string.IsNullOrWhiteSpace(request.CodeSnippet))
            {
                return BadRequest(new
                {
                    message = "No input provided."
                });
            }

            /* ---------- BASE ANALYSIS ---------- */

            var result = _service.Analyze(request.ErrorMessage);

            bool needsAi = _aiDecision.ShouldUseAI(result.confidence);
           
            string explanation =
                request.ExplanationMode == "beginner"
                ? "BEGINNER MODE:\n" + result.explanation
                : "DEVELOPER MODE:\n" + result.explanation;

            string suggestion = result.suggestion;

            /* ---------- SAVE HISTORY ---------- */

            _historyService.Add(new ErrorHistory
            {
                ErrorMessage = request.ErrorMessage,
                CodeSnippet = request.CodeSnippet,
                Explanation = explanation
            });
            if (needsAi)
            {
                var aiText = _openAi
                    .AnalyzeAsync(
                        request.ErrorMessage + "\n" + request.CodeSnippet,
                        request.ExplanationMode
                    ).Result;

                explanation += "\n\nAI Analysis:\n" + aiText;
            }
            /* ---------- CODE FIX GENERATION ---------- */

            var fixes = _codeFix.GenerateFix(
                request.ErrorMessage,
                request.CodeSnippet,
                request.ExplanationMode
            );

            /* ---------- RISK LEVEL ---------- */

            string riskLevel;

            if (result.confidence >= 80)
                riskLevel = "Low";
            else if (result.confidence >= 50)
                riskLevel = "Medium";
            else
                riskLevel = "High";

            /* ---------- LEARNING INSIGHT (WOW FEATURE) ---------- */

            var insight = _historyService
                .GetLearningInsights()
                .FirstOrDefault();

            /* ---------- RESPONSE ---------- */

            return Ok(new
            {
                riskLevel = riskLevel,
                confidence = result.confidence,
                explanation = explanation,
                suggestion = suggestion,
                fixedCode = fixes.FixedCode,
                alternativeFix = fixes.AlternativeFix,
                aiRequired = needsAi,
                insight = insight
            });
        }
    }
}