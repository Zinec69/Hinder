using idk.Helpers;
using idk.Models;
using idk.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace idk.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly DBContext _context;

        public AccountController(ILogger<AccountController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel user, string returnUrl)
        {
            var userDb = _context.User.FirstOrDefault(x => x.Email == user.Email);

            if (userDb == null)
            {
                ModelState.AddModelError("Email", "User with this email does not exist");
            }
            else if (!PasswordHasher.Verify(user.Password, userDb.Password))
            {
                ModelState.AddModelError("Password", "Wrong password");
            }
            else
            {
                await userDb.LogIn(HttpContext);

                return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? Redirect(returnUrl)
                    : RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel user, string returnUrl)
        {
            var userDb = _context.User.FirstOrDefault(x => x.Email == user.Email);

            if (userDb != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists");
            }
            else
            {
                _context.User.Add(new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = PasswordHasher.Hash(user.Password)
                });
                _context.SaveChanges();

                await userDb.LogIn(HttpContext);

                return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? Redirect(returnUrl)
                    : RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");

            return View();
        }
    }
}
