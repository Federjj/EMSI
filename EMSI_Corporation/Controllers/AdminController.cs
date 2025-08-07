using Microsoft.AspNetCore.Mvc;

namespace EMSI_Corporation.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminPrincipal()
        {
            return View();
        }
    }
}
