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
        [MaxLength(20,ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 20 znaków")]
        public string CategoryName { get; set; }

        public virtual ICollection<Offer> Offer { get; set; }
    }
}
