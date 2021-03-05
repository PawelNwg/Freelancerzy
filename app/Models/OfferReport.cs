using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class OfferReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }
        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        [ForeignKey("ReportingUser")]
        public int ReportingUserId { get; set; }
        public DateTime? ReportDate { get; set; }
        public int ReasonId { get; set; }
        public bool IsActive { get; set; }
        public virtual PageUser ReportingUser { get; set; }
        public virtual Offer Offer { get; set; }

        public virtual OfferReportReason OfferReportReason { get; set; }

    }
}
