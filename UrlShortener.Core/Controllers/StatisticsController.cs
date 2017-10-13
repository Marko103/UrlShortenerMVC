using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UrlShortener.Core.Models;

namespace UrlShortener.Core.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        public UrlShortenerContext Context;

        public StatisticsController(UrlShortenerContext c)
        {
            Context = c;
        }

        public IActionResult Index()
        {
            var shortUrlDatas = Context.ShortUrlDatas.Include("Statistic").ToList();

            return View(shortUrlDatas);
        }
    }
}