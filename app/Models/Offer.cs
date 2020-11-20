using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Offer
    {
        public int Offerid { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ViewCounter { get; set; }
        public decimal? Wage { get; set; }

        public virtual Category Category { get; set; }
        public virtual PageUser User { get; set; }
    }
}
