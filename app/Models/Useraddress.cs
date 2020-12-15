using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace freelancerzy.Models
{
    public partial class Useraddress
    {
        public int Userid { get; set; }
        [Display(Name = "Ulica")]
        [RegularExpression("^[a-zA-ZzżźćńółęąśŻŹĆĄŚĘŁÓŃ0-9]*$", ErrorMessage = "Niepoprawna nazwa ulicy")]
        public string Street { get; set; }
        [Display(Name = "Numer")]
        [RegularExpression("^[0-9]*[a-zA-Z]*$", ErrorMessage = "Niepoprawny numer ulicy")] //TODO: zmienić typ w bazie danych bo może być adres np 11C
        public int Number { get; set; }
        [Display(Name = "Numer mieszkania")]
        [RegularExpression("^[0-9]*[a-zA-Z]*$", ErrorMessage = "Niepoprawny numer mieszkania")] //TODO: zmienić typ w bazie danych bo może być adres np 11C
        public int? ApartmentNumber { get; set; }
        [Display(Name = "Kod pocztowy")] //TODO: regex do kodu pocztowego
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Miejscowość")]
        [RegularExpression("^[a-zA-ZzżźćńółęąśŻŹĆĄŚĘŁÓŃ]*$", ErrorMessage = "Nazwa miejscowości może zawierać tylko litery")]
        public string City { get; set; }

        public virtual PageUser User { get; set; }
    }
}
