using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class UserReportReason
    {
        public UserReportReason()
        {
            ReportedOffers = new HashSet<UserReport>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserReport> ReportedOffers { get; set; }
    }
}
