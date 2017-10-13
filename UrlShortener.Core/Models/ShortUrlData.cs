using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Core.Models
{
    public class ShortUrlData
    {
        [Key]
        public Guid Id { get; set; }

        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }
        public string UniquePart { get; set; }

        public Guid StatisticId { get; set; }
        public virtual Statistic Statistic { get; set; }

        public ShortUrlData()
        {
            Id = Guid.NewGuid();
        }
    }
}
