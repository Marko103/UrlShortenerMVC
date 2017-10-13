using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Core.Models
{
    public class UrlShortenerContext : IdentityDbContext<ApplicationUser>
    {
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options)
        {
        }

        public UrlShortenerContext()
        {
        }

        public DbSet<ShortUrlData> ShortUrlDatas { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
    }
}