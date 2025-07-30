using Microsoft.AspNetCore.Mvc;

namespace CodeZone_Task.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
