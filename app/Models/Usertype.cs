using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace freelancerzy.Models
{
    public partial class Usertype
    {
        public Usertype()
        {
            PageUser = new HashSet<PageUser>();
        }

        public int Typeid { get; set; }
        [Display(Name = "Typ")]
        public string Name { get; set; }

        public virtual ICollection<PageUser> PageUser { get; set; }
    }
}
