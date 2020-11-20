using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Category
    {
        public Category()
        {
            Offer = new HashSet<Offer>();
        }

        public int Categoryid { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Offer> Offer { get; set; }
    }
}
