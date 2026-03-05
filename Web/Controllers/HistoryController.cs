using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ErrorHistoryService _historyService;

        public HistoryController(ErrorHistoryService historyService)
        {
            _historyService = historyService;
        }
        private string ExtractExceptionType(string message)
        {
            if (string.IsNullOrEmpty(message))
                return "Unknown";

            if (message.Contains(":"))
                message = message.Split(':')[0];

            if (message.Contains("."))
                message = message.Split('.').Last();

            return message.Trim();
        }
        public IActionResult Index()
        {
            var history = _historyService.GetAll();

            var commonErrors = history
                .GroupBy(x => x.ErrorMessage)
                .Select(g => new CommonErrorViewModel
                {
                    Error = g.Key,
                    Count = g.Count(),
                    LastActivity = g.Max(x => x.Date)
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var trendInsights = history
                .GroupBy(x => x.ErrorMessage)
                .Select(g => $"{g.Key} appears {g.Count()} times.")
                .ToList();

            var warnings = history
                .GroupBy(x => x.ErrorMessage)
                .Where(g => g.Count() >= 3)
                .Select(g => $"You often encounter: {g.Key}")
                .ToList();

            var insights = _historyService.GetLearningInsights();
            var stats1 = _historyService.GetDashboardStats();
            var progress = _historyService.GetProgressInsights();
            var behavior = _historyService.GetBehaviorInsights();

            var skillService = HttpContext.RequestServices
                .GetService<SkillAssessmentService>();

            var model = new Web.Models.HistoryViewModel
            {
                Errors = history,
                CommonErrors = commonErrors,
                Warnings = warnings,
                LearningInsights = insights,
                TrendInsights = trendInsights,
                Stats = stats1,
                ProgressInsights = progress,
                Behavior = behavior,
                SkillLevel = skillService?.GetSkillLevel()
            };

            return View(model);
        }

    }
}
