using Microsoft.AspNetCore.Mvc;

namespace Parbad.Sample.CustomStorage.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
