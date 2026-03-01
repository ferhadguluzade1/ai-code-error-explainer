using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ErrorHistoryService _historyService;

        public HomeController(ErrorHistoryService historyService)
        {
            _historyService = historyService;
        }
        public IActionResult Index()
        {
            var history = _historyService.GetAll();

            ViewBag.SystemStatus = DateTime.Now.Second % 2 == 0
    ? "Online"
    : "Operational";
            ViewBag.TotalAnalyses = history.Count;

            if (history.Count >= 5)
                ViewBag.LearningState = "Improving";
            else
                ViewBag.LearningState = "Getting Started";

            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}