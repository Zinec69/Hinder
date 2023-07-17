using Microsoft.AspNetCore.Mvc;

namespace idk.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult Index() => View();
    }
}
