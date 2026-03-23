using Microsoft.AspNetCore.Mvc;

namespace Payzo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        // If already logged in, go straight to dashboard
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");
        return View();
    }
}
