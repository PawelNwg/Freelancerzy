using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace freelancerzy.Models
{
    public partial class Credentials
    {
        public int Userid { get; set; }

        [Required(ErrorMessage ="Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Hasło musi posiadać min 8 znaków, wielkie litery, małe oraz znaki specjalne")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Hasła nie są identyczne")]
        [Display(Name = "Powtórz hasło")]
        public string PasswordConfirmed { get; set; }
        
        [NotMapped]
        //[Required(ErrorMessage = "Stare hasła jest wymagane")] TODO
        [DataType(DataType.Password)]
        [Display(Name = "Stare hasło")]
        public string OldPassword{ get; set; }
        public virtual PageUser User { get; set; }
    }
}
