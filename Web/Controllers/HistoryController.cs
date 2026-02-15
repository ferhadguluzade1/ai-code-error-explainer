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

        public IActionResult Index()
        {
            var history = _historyService.GetAll();

            var mostCommonErrors = history
                .GroupBy(x => x.ErrorMessage)
                .Select(g => new
                {
                    Error = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            ViewBag.CommonErrors = mostCommonErrors;

            return View(history);
        }

    }
}
