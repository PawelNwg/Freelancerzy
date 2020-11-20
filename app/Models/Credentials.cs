using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Credentials
    {
        public int Userid { get; set; }
        public string Password { get; set; }

        public virtual PageUser User { get; set; }
    }
}
