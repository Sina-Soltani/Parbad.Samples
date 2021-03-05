using Microsoft.AspNetCore.Mvc;

namespace Parbad.Sample.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
