using System.Collections.Generic;
using System.Linq;
using Web.Data;

namespace Web.Models
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
    }
}