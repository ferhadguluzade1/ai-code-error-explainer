using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

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
        /*
        private readonly OpenAiService _openAi;*/

        /*
        public ErrorApiController(
            ErrorHistoryService historyService,
            OpenAiService openAi)
        {
            _service = new ErrorAnalysisService();
            _historyService = historyService;
            _codeFix = new CodeFixService();
            _openAi = openAi;
        }*/
        public ErrorApiController(ErrorHistoryService historyService)
        {
            _service = new ErrorAnalysisService();
            _aiDecision = new AiDecisionService();
            _historyService = historyService;
            _codeFix = new CodeFixService();
        }
        /*
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

            string explanation;
            string suggestion;
            int confidence;
            string riskLevel;

            if (result.confidence <= 0)
            {
                explanation = "Input could not be classified as a known exception.";
                suggestion = "Provide a valid .NET exception message.";
                confidence = 40;
                riskLevel = "Medium";
            }
            else
            {
                explanation = request.ExplanationMode == "beginner"
                    ? "BEGINNER MODE:\n" + result.explanation
                    : "DEVELOPER MODE:\n" + result.explanation;

                suggestion = result.suggestion;
                confidence = result.confidence;

                riskLevel =
                    confidence >= 80 ? "Low" :
                    confidence >= 50 ? "Medium" :
                    "High";
            }

            var fixes = _codeFix.GenerateFix(
                request.ErrorMessage,
                request.CodeSnippet,
                request.ExplanationMode
            );

            // 🔥 HISTORY ALWAYS SAVED
            _historyService.Add(new ErrorHistory
            {
                ErrorMessage = request.ErrorMessage,
                CodeSnippet = request.CodeSnippet,
                Explanation = explanation,
                Date = DateTime.Now
            });

            return Ok(new
            {
                riskLevel,
                confidence,
                explanation,
                suggestion,
                fixedCode = fixes?.FixedCode ?? "",
                alternativeFix = fixes?.AlternativeFix ?? "",
                aiRequired = false
            });
        }*/

        /*
        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            try
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

                string explanation = request.ExplanationMode == "beginner"
                    ? "BEGINNER MODE:\n" + result.explanation
                    : "DEVELOPER MODE:\n" + result.explanation;

                string suggestion = result.suggestion;

                var fixes = _codeFix.GenerateFix(
                    request.ErrorMessage,
                    request.CodeSnippet,
                    request.ExplanationMode
                );

                int confidence = result.confidence;

                string riskLevel =
                    confidence >= 80 ? "Low" :
                    confidence >= 50 ? "Medium" :
                    "High";

                _historyService.Add(new ErrorHistory
                {
                    ErrorMessage = request.ErrorMessage,
                    CodeSnippet = request.CodeSnippet,
                    Explanation = explanation,
                    Date = DateTime.Now
                });

                return Ok(new
                {
                    riskLevel,
                    confidence,
                    explanation,
                    suggestion,
                    fixedCode = fixes?.FixedCode ?? "",
                    alternativeFix = fixes?.AlternativeFix ?? "",
                    aiRequired = false
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    riskLevel = "Medium",
                    confidence = 40,
                    explanation = "Unexpected input. The analyzer could not process this error.",
                    suggestion = "Try providing a recognizable .NET exception.",
                    fixedCode = "",
                    alternativeFix = "",
                    aiRequired = false
                });
            }
        }*/
        /*
        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            try
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

                if (result.confidence <= 0)
                {
                    return Ok(new
                    {
                        riskLevel = "Medium",
                        confidence = 40,
                        explanation = "Input could not be classified as a known exception.",
                        suggestion = "Provide a valid .NET exception message.",
                        fixedCode = "",
                        alternativeFix = "",
                        aiRequired = false
                    });
                }

                string explanation =
                    request.ExplanationMode == "beginner"
                    ? "BEGINNER MODE:\n" + result.explanation
                    : "DEVELOPER MODE:\n" + result.explanation;

                string suggestion = result.suggestion;

                var fixes = _codeFix.GenerateFix(
                    request.ErrorMessage,
                    request.CodeSnippet,
                    request.ExplanationMode
                );

                string fixedCode = fixes?.FixedCode ?? "";
                string alternativeFix = fixes?.AlternativeFix ?? "";

                string riskLevel =
                    result.confidence >= 80 ? "Low" :
                    result.confidence >= 50 ? "Medium" :
                    "High";

                // SAVE HISTORY
                _historyService.Add(new ErrorHistory
                {
                    ErrorMessage = request.ErrorMessage,
                    CodeSnippet = request.CodeSnippet,
                    Explanation = explanation,
                    Date = DateTime.Now
                });

                return Ok(new
                {
                    riskLevel = riskLevel,
                    confidence = result.confidence,
                    explanation = explanation,
                    suggestion = suggestion,
                    fixedCode = fixedCode,
                    alternativeFix = alternativeFix,
                    aiRequired = false
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Server error",
                    detail = ex.Message
                });
            }
        }
        */
        /*
        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is null");
                }

                if (string.IsNullOrWhiteSpace(request.ErrorMessage)
                    && string.IsNullOrWhiteSpace(request.CodeSnippet))
                {
                    return BadRequest("No input provided");
                }

                var result = _service.Analyze(request.ErrorMessage);

                string explanation = "";
                string suggestion = "";
                int confidence = 50;

                if (result.confidence > 0)
                {
                    explanation = request.ExplanationMode == "beginner"
                        ? "BEGINNER MODE:\n" + result.explanation
                        : "DEVELOPER MODE:\n" + result.explanation;

                    suggestion = result.suggestion;
                    confidence = result.confidence;
                }
                else
                {
                    explanation = "Input could not be classified.";
                    suggestion = "Provide a valid .NET exception.";
                }

                var fixes = _codeFix.GenerateFix(
                    request.ErrorMessage,
                    request.CodeSnippet,
                    request.ExplanationMode
                );

                string fixedCode = fixes?.FixedCode ?? "";
                string alternativeFix = fixes?.AlternativeFix ?? "";

                string riskLevel =
                    confidence >= 80 ? "Low" :
                    confidence >= 50 ? "Medium" :
                    "High";

                // HISTORY SAVE (SAFE)
                try
                {
                    _historyService.Add(new ErrorHistory
                    {
                        ErrorMessage = request.ErrorMessage,
                        CodeSnippet = request.CodeSnippet,
                        Explanation = explanation,
                        Date = DateTime.Now
                    });
                }
                catch { }

                return Ok(new
                {
                    riskLevel = riskLevel,
                    confidence = confidence,
                    explanation = explanation,
                    suggestion = suggestion,
                    fixedCode = fixedCode,
                    alternativeFix = alternativeFix,
                    aiRequired = false
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Server crash",
                    detail = ex.Message
                });
            }
        }
        */
        /*
        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            try
            {
                return Ok(new
                {
                    riskLevel = "Low",
                    confidence = 90,
                    explanation = "Test response from API",
                    suggestion = "Controller works",
                    fixedCode = "",
                    alternativeFix = "",
                    aiRequired = false
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
        */
        /*
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

            string explanation;
            string suggestion;
            int confidence;

            if (result.confidence <= 0)
            {
                explanation = "Input could not be classified as a known exception.";
                suggestion = "Provide a valid .NET exception message.";
                confidence = 40;
            }
            else
            {
                explanation = request.ExplanationMode == "beginner"
                    ? "BEGINNER MODE:\n" + result.explanation
                    : "DEVELOPER MODE:\n" + result.explanation;

                suggestion = result.suggestion;
                confidence = result.confidence;
            }

            var fixes = _codeFix.GenerateFix(
                request.ErrorMessage,
                request.CodeSnippet,
                request.ExplanationMode
            );

            string riskLevel =
                confidence >= 80 ? "Low" :
                confidence >= 50 ? "Medium" :
                "High";

            // HISTORY SAVE (ARTIQ HƏR HALDA İŞLƏYİR)
            _historyService.Add(new ErrorHistory
            {
                ErrorMessage = request.ErrorMessage,
                CodeSnippet = request.CodeSnippet,
                Explanation = explanation,
                Date = DateTime.Now
            });

            return Ok(new
            {
                riskLevel = riskLevel,
                confidence = confidence,
                explanation = explanation,
                suggestion = suggestion,
                fixedCode = fixes?.FixedCode,
                alternativeFix = fixes?.AlternativeFix,
                aiRequired = false
            });
        }*/

        /*
        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ErrorMessage)
                    && string.IsNullOrWhiteSpace(request.CodeSnippet))
                {
                    return BadRequest(new { message = "No input provided." });
                }

                var result = _service.Analyze(request.ErrorMessage);

                string explanation;
                string suggestion;
                int confidence;

                if (result.confidence <= 0)
                {
                    explanation = "Input could not be classified as a known exception.";
                    suggestion = "Provide a valid .NET exception message.";
                    confidence = 40;
                }
                else
                {
                    explanation = request.ExplanationMode == "beginner"
                        ? "BEGINNER MODE:\n" + result.explanation
                        : "DEVELOPER MODE:\n" + result.explanation;

                    suggestion = result.suggestion;
                    confidence = result.confidence;
                }
                bool recognized = result.confidence > 30;
                var fixes = _codeFix.GenerateFix(
      request.ErrorMessage,
      request.CodeSnippet,
      request.ExplanationMode
  );

                string riskLevel =
                    confidence >= 80 ? "Low" :
                    confidence >= 50 ? "Medium" :
                    "High";

                _historyService.Add(new ErrorHistory
                {
                    ErrorMessage = request.ErrorMessage,
                    CodeSnippet = request.CodeSnippet,
                    Explanation = explanation,
                    Date = DateTime.Now
                });

                return Ok(new
                {
                    riskLevel,
                    confidence,
                    explanation,
                    suggestion,
                    fixedCode = recognized ? fixes?.FixedCode : null,
                    alternativeFix = recognized ? fixes?.AlternativeFix : null,
                    aiRequired = false
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    riskLevel = "Medium",
                    confidence = 50,
                    explanation = "System handled an unexpected error safely.",
                    suggestion = "Try again with a clearer exception message.",
                    fixedCode = "",
                    alternativeFix = "",
                    aiRequired = false
                });
            }
        }
        */
        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] ErrorAnalysisRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ErrorMessage)
                    && string.IsNullOrWhiteSpace(request.CodeSnippet))
                {
                    return BadRequest(new { message = "No input provided." });
                }

                var result = _service.Analyze(request.ErrorMessage);

                string explanation;
                string suggestion;
                int confidence;

                /* ---------- UNKNOWN INPUT ---------- */

                if (result.confidence <= 0)
                {
                    explanation = "Input could not be classified as a known .NET exception.";
                    suggestion = "Provide a valid exception message or code snippet.";
                    confidence = 0;
                }
                else
                {
                    explanation = request.ExplanationMode == "beginner"
                        ? "BEGINNER MODE:\n" + result.explanation
                        : "DEVELOPER MODE:\n" + result.explanation;

                    suggestion = result.suggestion;
                    confidence = result.confidence;
                }

                /* ---------- RECOGNITION FLAG ---------- */

                bool recognized = confidence > 30;
                bool hasCode = !string.IsNullOrWhiteSpace(request.CodeSnippet);

                /* ---------- FIX GENERATION ---------- */
                FixResult fixes = null;

                if (recognized && hasCode)
                {
                    fixes = _codeFix.GenerateFix(
                        request.ErrorMessage,
                        request.CodeSnippet,
                        request.ExplanationMode
                    );
                }
                /* ---------- RISK LEVEL ---------- */

                string riskLevel =
                    confidence >= 80 ? "Low" :
                    confidence >= 50 ? "Medium" :
                    confidence > 0 ? "High" :
                    "Unknown";

                /* ---------- SAVE HISTORY ---------- */

                _historyService.Add(new ErrorHistory
                {
                    ErrorMessage = request.ErrorMessage,
                    CodeSnippet = request.CodeSnippet,
                    Explanation = explanation,
                    Date = DateTime.Now
                });

                /* ---------- RESPONSE ---------- */

                return Ok(new
                {
                    riskLevel,
                    confidence,
                    explanation,
                    suggestion,
                    fixedCode = fixes?.FixedCode,
                    alternativeFix = fixes?.AlternativeFix,
                    aiRequired = false
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    riskLevel = "Medium",
                    confidence = 50,
                    explanation = "System handled an unexpected error safely.",
                    suggestion = "Try again with a clearer exception message.",
                    fixedCode = "",
                    alternativeFix = "",
                    aiRequired = false
                });
            }
        }
    }
}