using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class OfferReportReason
    {
        public OfferReportReason()
        {
            OfferReports = new HashSet<OfferReport>();
        }
        public int ReasonId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<OfferReport> OfferReports { get; set; }
    }
}
