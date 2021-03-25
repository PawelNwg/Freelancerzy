using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class UserReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }
        [ForeignKey("ReportedUser")]
        public int UserId { get; set; }
        [ForeignKey("ReportingUser")]
        public int ReportingUserId { get; set; }
        public DateTime? ReportDate { get; set; }
        public int ReasonId { get; set; }
        public bool IsActive { get; set; }

        public virtual PageUser ReportingUser { get; set; }
        public virtual PageUser ReportedUser { get; set; }
        public virtual OfferReportReason OfferReportReason { get; set; }
    }
}
