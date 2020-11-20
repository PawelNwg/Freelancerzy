using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Permission
    {
        public Permission()
        {
            Permissionuser = new HashSet<Permissionuser>();
        }

        public int Permissionid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Permissionuser> Permissionuser { get; set; }
    }
}
