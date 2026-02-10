using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.MVC
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
