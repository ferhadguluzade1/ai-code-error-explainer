using System.Collections.Generic;
using System.Linq;
using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class ErrorHistoryService
    {
        private readonly AppDbContext _context;

        public ErrorHistoryService(AppDbContext context)
        {
            _context = context;
        }

        public void Add(ErrorHistory error)
        {
            /*
            _context.ErrorHistories.Add(error);
            //_context.SaveChanges();
            */
           
            _context.ErrorHistories.Add(error);
            _context.SaveChanges();

            
        }

        public List<ErrorHistory> GetAll()
        {
            return _context.ErrorHistories
                .OrderByDescending(x => x.Date)
                .ToList();
        }
        public List<string> GetLearningInsights()
        {
            var history = _context.ErrorHistories.ToList();

            var normalized = history
                .Select(x => new
                {
                    ExceptionType = ExtractExceptionType(x.ErrorMessage)
                });

            var insights = normalized
                .GroupBy(x => x.ExceptionType)
                .Where(g => g.Count() >= 2)
                .Select(g => $"You encountered '{g.Key}' {g.Count()} times. Consider mastering this concept.")
                .ToList();

            return insights;
        }
        public List<string> GetProgressInsights()
        {
            var history = _context.ErrorHistories
                .OrderBy(x => x.Date)
                .ToList();

            var grouped = history
                .GroupBy(x => ExtractExceptionType(x.ErrorMessage));

            var insights = new List<string>();

            foreach (var g in grouped)
            {
                if (g.Count() < 3)
                    continue;

                var firstHalf = g.Take(g.Count() / 2).Count();
                var secondHalf = g.Skip(g.Count() / 2).Count();

                if (secondHalf < firstHalf)
                {
                    insights.Add(
                        $"Improvement detected: '{g.Key}' errors are decreasing over time."
                    );
                }
            }

            return insights;
        }

        private string ExtractExceptionType(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return "Unknown";

            if (errorMessage.Contains(":"))
                errorMessage = errorMessage.Split(':')[0];

            if (errorMessage.Contains("."))
                errorMessage = errorMessage.Split('.').Last();

            return errorMessage.Trim();
        }

        public string BuildLearningContext()
        {
            var history = _context.ErrorHistories
                .OrderByDescending(x => x.Date)
                .Take(5)
                .ToList();

            if (!history.Any())
                return "No previous learning history.";

            var context = "Recent learning context:\n";

            foreach (var item in history)
            {
                context += $"- {ExtractExceptionType(item.ErrorMessage)}\n";
            }

            return context;
        }
        public object GetUserStats()
        {
            var history = _context.ErrorHistories.ToList();

            var total = history.Count;

            var mostCommon = history
                .GroupBy(x => ExtractExceptionType(x.ErrorMessage))
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "None";

            var lastActivity = history
                .OrderByDescending(x => x.Date)
                .Select(x => x.Date)
                .FirstOrDefault();

            string level =
                total < 5 ? "Beginner" :
                total < 15 ? "Learning" :
                total < 30 ? "Improving" :
                "Advanced Debugger";

            return new
            {
                Total = total,
                MostCommon = mostCommon,
                LastActivity = lastActivity,
                Level = level
            };
        }

        public object GetDashboardStats()
        {
            var history = _context.ErrorHistories.ToList();

            var total = history.Count;

            var mostCommon = history
                .GroupBy(x => ExtractExceptionType(x.ErrorMessage))
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "None";

            var learningScore = total == 0
                ? 0
                : Math.Min(100, total * 5); // sadə demo metric

            return new
            {
                Total = total,
                MostCommon = mostCommon,
                Score = learningScore
            };
        }
        public object GetBehaviorInsights()
        {
            var history = _context.ErrorHistories.ToList();

            if (!history.Any())
                return null;

            var total = history.Count;

            var mostCommon = history
                .GroupBy(x => ExtractExceptionType(x.ErrorMessage))
                .OrderByDescending(g => g.Count())
                .First();

            var recent = history
                .OrderByDescending(x => x.Date)
                .Take(5)
                .Select(x => ExtractExceptionType(x.ErrorMessage))
                .ToList();

            var older = history
                .OrderBy(x => x.Date)
                .Take(5)
                .Select(x => ExtractExceptionType(x.ErrorMessage))
                .ToList();

            bool improving = older.Count(e => e == mostCommon.Key)
                             >
                             recent.Count(e => e == mostCommon.Key);

            return new
            {
                TotalAnalyses = total,
                MostCommonError = mostCommon.Key,
                Count = mostCommon.Count(),
                Improving = improving
            };
        }
    }
}