using idk.Models;
using idk.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace idk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _context;

        public HomeController(ILogger<HomeController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var id = int.Parse(
                HttpContext.User.Claims
                .First(x => x.Type == "ID")
                .Value);
            var user = _context.User.Single(x => x.ID == id);
            var users = user.GetNextUnmatchedUsers(_context, count: 2);

            return View(users);
        }

        [HttpGet]
        public IActionResult GetNextUser()
        {
            var id = int.Parse(
                HttpContext.User.Claims
                .First(x => x.Type == "ID")
                .Value);
            var users = _context.User
                .Single(x => x.ID == id)
                .GetNextUnmatchedUsers(_context);

            return PartialView("_ProfilePartial", users.Count > 0 ? users[0] : null);
        }

        [HttpPost]
        public void SubmitSwipe(int userId, bool smash)
        {
            var id = int.Parse(
                HttpContext.User.Claims
                .First(x => x.Type == "ID")
                .Value);

            _context.UserLikes.Add(new UserLikes
            {
                UserID1 = id,
                UserID2 = userId,
                Smash = smash
            });
            _context.SaveChanges();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}