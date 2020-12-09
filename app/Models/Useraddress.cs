using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace freelancerzy.Models
{
    public partial class Useraddress
    {
        public int Userid { get; set; }
        [Display(Name = "Ulica")]
        [RegularExpression("^[A-Z][a-zA-Z ]*$", ErrorMessage = "Nazwa ulicy może zawierać tylko litery")]
        public string Street { get; set; }
        [Display(Name = "Numer")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numer może zawierać tylko cyfry")]
        public int Number { get; set; }
        [Display(Name = "Numer mieszkania")]
        [Range(0, 100000, 
        ErrorMessage = "Numer mieszkania może zawierać tylko cyfry")]
        public int? ApartmentNumber { get; set; }
        [Display(Name = "Kod pocztowy")] //TODO: regex do kodu pocztowego
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Miejscowość")]
        [RegularExpression("^[A-Z][a-zA-Z ]*$", ErrorMessage = "Nazwa miejscowości może zawierać tylko litery")]
        public string City { get; set; }

        public virtual PageUser User { get; set; }
    }
}
