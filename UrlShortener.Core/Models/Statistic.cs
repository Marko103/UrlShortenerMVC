using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Core.Models
{
    public class Statistic
    {
        [Key]
        public Guid Id { get; set; }

        public int NumOfClicks { get; set; }

        public Statistic()
        {
            Id = Guid.NewGuid();
        }
    }
}
