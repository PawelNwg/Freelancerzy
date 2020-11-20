using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Reason
    {
        public Reason()
        {
            Messagereport = new HashSet<Messagereport>();
        }

        public int Reasonid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Messagereport> Messagereport { get; set; }
    }
}
