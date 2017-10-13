using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Models;
using UrlShortener.Core.Utils;

namespace UrlShortener.Core.Controllers
{
    [Authorize]
    public class UrlController : Controller
    {
        private UrlShortenerContext Context;

        public UrlController(UrlShortenerContext c)
        {
            Context = c;
        }

        public IActionResult Index()
        {
            var shortUrls = Context.ShortUrlDatas.ToList();

            return View(shortUrls);
        }

        public async Task<IActionResult> CreateShortUrl(string originalUrl)
        {
            if(originalUrl == null || originalUrl.Equals(string.Empty)) return RedirectToAction("Index");

            ShortUrlData newShortUrl;
            try
            {
                newShortUrl = await UrlManager.ShortUrl(Request, Context, originalUrl.ToLower());
            }
            catch (ShortUrlCreationException)
            {
                return ShowErrorMessage("Short URL could not be created");
                
            }
            catch (ShortUrlExistsException)
            {
                return ShowErrorMessage($"Short URL already exists for {originalUrl}");
            }
            catch (ShortUrlNotUrlException)
            {
                return ShowErrorMessage("Given input is not an URL");
            }

            return RedirectToAction("Index");
        }
        
        public IActionResult ToUrl(string id)
        {
            var shortUrlData = Context.ShortUrlDatas.Include("Statistic").FirstOrDefault(su => su.UniquePart.Equals(id));
            if (shortUrlData == null)
            {
                ViewData["ErrorMessage"] = $"Short URL not found";
                return View("Index");
                //return RedirectToAction("Index");
            }

            shortUrlData.Statistic = shortUrlData.Statistic ?? new Statistic();

            shortUrlData.Statistic.NumOfClicks++;

            Context.SaveChanges();

            return Redirect(shortUrlData.LongUrl);
        }

        private IActionResult ShowErrorMessage(string message)
        {
            ViewData["ErrorMessage"] = message;
            var shortUrls = Context.ShortUrlDatas.ToList();

            return View("Index", shortUrls);
        }
    }
}