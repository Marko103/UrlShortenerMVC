using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Models;

namespace UrlShortener.Core.Utils
{
    public class UrlManager
    {
        public static Task<ShortUrlData> ShortUrl(HttpRequest request, UrlShortenerContext context, string longUrl)
        {
            return Task.Run(() =>
            {
                bool isUrl = Uri.TryCreate(longUrl, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (!isUrl) throw new ShortUrlNotUrlException();

                var existingUrl = context.ShortUrlDatas.FirstOrDefault(sud => sud.LongUrl.Equals(uriResult.AbsoluteUri));
                if (existingUrl != null) throw new ShortUrlExistsException();

                var uniquePart = GetUniquePart(context);
                var shortUrl = GetShortUrl(request, uniquePart);

                var shortUrlData = new ShortUrlData
                {
                    LongUrl = uriResult.AbsoluteUri,
                    ShortUrl = shortUrl,
                    UniquePart = uniquePart,
                    Statistic = new Statistic()
                };

                context.ShortUrlDatas.Add(shortUrlData);

                context.SaveChanges();

                return shortUrlData;
            });
        }

        private static string GetUniquePart(UrlShortenerContext context)
        {
            var uniquePart = string.Empty;

            for(var i = 0; i < 10; i++)
            {
                uniquePart = Guid.NewGuid().ToString("N").Replace('-', ' ').Substring(0, 10);
                var existingPart = context.ShortUrlDatas.FirstOrDefault(sud => sud.UniquePart.Equals(uniquePart));
                if (existingPart == null) break;
            }

            if (uniquePart.Equals(string.Empty)) throw new ShortUrlCreationException(); 

            return uniquePart;
        }

        private static string GetShortUrl(HttpRequest request, string uniquePart)
        {
            var host = request.Host.Value;
            var controller = "Redirect";
            var action = "ToUrl";
            var id = uniquePart;

            return string.Join("/", host, controller, action, id);
        }
    }
}
