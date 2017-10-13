using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using UrlShortener.Core.Models;

namespace UrlShortener.Core.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}