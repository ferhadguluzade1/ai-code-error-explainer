using Microsoft.AspNetCore.Mvc;
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

            var mostCommonErrors = history
      .GroupBy(x => x.ErrorMessage)
      .Select(g => new
      {
          Error = g.Key,
          Count = g.Count(),
          LastActivity = g.Max(x => x.Date)
      })
      .OrderByDescending(x => x.Count)
      .ToList();


            ViewBag.CommonErrors = mostCommonErrors;

            var trendInsights = history
    .GroupBy(x => ExtractExceptionType(x.ErrorMessage))
    .Select(g => new
    {
        Error = g.Key,
        Count = g.Count()
    })
    .OrderByDescending(x => x.Count)
    .Take(3)
    .Select(x =>
    {
        if (x.Count >= 5)
            return $"⚠ You frequently encounter '{x.Error}'. Focus on mastering it.";

        if (x.Count >= 3)
            return $"📈 '{x.Error}' appears repeatedly — improvement opportunity.";

        return $"✅ '{x.Error}' appears under control.";
    })
    .ToList();

            ViewBag.TrendInsights = trendInsights;
            var warnings = history
            .GroupBy(x => x.ErrorMessage)
            .Where(g => g.Count() >= 3)
            .Select(g => $"You often encounter: {g.Key}")
            .ToList();

            ViewBag.Warnings = warnings;
            var insights = _historyService.GetLearningInsights();
            ViewBag.LearningInsights = insights;
            var stats = _historyService.GetUserStats();
            ViewBag.Stats = stats;
            var progress = _historyService.GetProgressInsights();
            ViewBag.ProgressInsights = progress;
            var stats1 = _historyService.GetDashboardStats();
            ViewBag.Stats = stats1;
            var behavior = _historyService.GetBehaviorInsights();
            ViewBag.Behavior = behavior;
            var skillService =
    HttpContext.RequestServices.GetService<SkillAssessmentService>();

            ViewBag.SkillLevel = skillService.GetSkillLevel();
            return View(history);
        }

    }
}
