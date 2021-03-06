using Microsoft.AspNetCore.Mvc;

namespace Parbad.Sample.EntityFrameworkCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
