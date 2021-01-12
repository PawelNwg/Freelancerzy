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
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }
        [MaxLength(40, ErrorMessage = "Tytuł może mieć maksymalnie 40 znaków")]
        [Required(ErrorMessage = "Tytuł jest obowiązkowy")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "Opis może mieć maksymalnie 1500 znaków")]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Data utworzenia")] // nie do edycji
        public DateTime CreationDate { get; set; }
        [Display(Name = "Data ostatniej zmiany")] //Też nie do edycji chyba
        public DateTime? LastModificationDate { get; set; }
        [Display(Name = "Data wygaśnięcia")]
        public DateTime ExpirationDate { get; set; }
        [Display(Name = "Ilość wyświetleń")]
        public int ViewCounter { get; set; }

        [Display(Name = "Stawka")]
        [DataType(DataType.Currency)]
        //TODO: zrobić coś żeby ładnie wyświetlało z dwoma miejscami po przecinku
        public decimal? Wage { get; set; }
        [NotMapped]
        [RegularExpression(@"^([0-9]*)([.,]*)([0-9]\d{0,1})$", ErrorMessage = "Zły format")]
        [Display(Name = "Stawka")]
        public string WageValue { get; set; }
        public virtual Category Category { get; set; }
        public virtual PageUser User { get; set; }
    }
}
