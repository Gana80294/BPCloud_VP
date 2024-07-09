using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.AuthenticatioService.Models
{
    public class ApplicationTour
    {
        public Guid UserId { get; set; }
        public bool TourStatus { get; set; }
    }
}
