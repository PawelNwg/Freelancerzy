using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace freelancerzy.Models
{
    public partial class Category
    {
        public Category()
        {
            Offer = new HashSet<Offer>();
        }

        public int Categoryid { get; set; }
        [Display(Name = "Nazwa")]
        public string CategoryName { get; set; }

        public virtual ICollection<Offer> Offer { get; set; }
    }
}
