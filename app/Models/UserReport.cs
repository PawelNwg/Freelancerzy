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
        //[ForeignKey("ReportedInUser")]
        public int UserReportedId { get; set; }
        //[ForeignKey("ReportedByUser")]
        public int UserReporterId { get; set; }
        public DateTime? ReportDate { get; set; }
        public bool IsActive { get; set; }
        public int ReportReasonId { get; set; }
        public UserReportReason ReportReason { get; set; }
        public virtual PageUser UserReported { get; set; }
        public virtual PageUser UserReporter { get; set; }
        
    }
   
}
