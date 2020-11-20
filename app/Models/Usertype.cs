using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Usertype
    {
        public Usertype()
        {
            PageUser = new HashSet<PageUser>();
        }

        public int Typeid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PageUser> PageUser { get; set; }
    }
}
