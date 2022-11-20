using Microsoft.AspNetCore.Mvc;

namespace Parbad.Sample.Docker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
