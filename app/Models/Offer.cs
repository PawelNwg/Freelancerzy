using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace freelancerzy.Models
{
    public partial class Offer
    {
        public int Offerid { get; set; }
        public int UserId { get; set; }
        
        public int CategoryId { get; set; }
        [MaxLength(40,ErrorMessage ="Tytuł może mieć maksymalnie 40 znaków")]
        [Required(ErrorMessage ="Tytuł jest obowiązkowy")]
        public string Title { get; set; }
        [MaxLength(250,ErrorMessage ="Opis może mieć maksymalnie 250 znaków")]
        public string Description { get; set; }
        
        public DateTime CreationDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ViewCounter { get; set; }
        [Display(Name = "Stawka")]
        
        [DataType(DataType.Currency)]
        public decimal? Wage { get; set; }
        [NotMapped]
        [RegularExpression(@"^([1-9]\d*|0)(\,\d+)?$", ErrorMessage = "Zły format")]
        public string WageValue { get; set; }
        public virtual Category Category { get; set; }
        public virtual PageUser User { get; set; }
    }
}
