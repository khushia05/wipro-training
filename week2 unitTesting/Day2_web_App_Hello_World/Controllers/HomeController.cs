using Microsoft.AspNetCore.Mvc;

namespace Day2_web_App_Hello_World.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
