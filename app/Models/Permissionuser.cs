using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Permissionuser
    {
        public int Userid { get; set; }
        public int Permissionid { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual PageUser User { get; set; }
    }
}
