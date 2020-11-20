using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Messagereport
    {
        public int Reportid { get; set; }
        public int MessageId { get; set; }
        public int ReasonId { get; set; }

        public virtual Message Message { get; set; }
        public virtual Reason Reason { get; set; }
    }
}
