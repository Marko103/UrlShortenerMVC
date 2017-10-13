using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Core.Models;

namespace UrlShortener.Core.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm)
        {
            _userManager = um;
            _signInManager = sm;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.InvariantCulture));
            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User '{username}' not found";
                return View("Index", ViewData);
            }

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (!passwordSignInResult.Succeeded)
            {
                ViewData["ErrorMessage"] = "Login failed";
                return View("Index", ViewData);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Register(string username, string password/*, string email*/)
        {
            var newUser = new ApplicationUser
            {
                UserName = username,
                //Email = email
            };

            var userCreated = await _userManager.CreateAsync(newUser, password);

            if (userCreated.Succeeded) return await Login(username, password);

            ViewData["ErrorMessages"] = userCreated.Errors.Select(e => e.Description);

            return View(ViewData);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}