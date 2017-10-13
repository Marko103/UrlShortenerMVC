using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Core.Models
{
    public class ApplicationUser: IdentityUser
    {
        public virtual ICollection<ShortUrlData> ShortUrlDatas { get; set; }
    }
}
