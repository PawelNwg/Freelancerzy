using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySql.Data.EntityFrameworkCore.DataAnnotations;

namespace freelancerzy.Models
{
    public partial class PageUser
    {
        public PageUser()
        {
            MessageUserFrom = new HashSet<Message>();
            MessageUserTo = new HashSet<Message>();
            Offer = new HashSet<Offer>();
            Permissionuser = new HashSet<Permissionuser>();
        }

        public int Userid { get; set; }
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane")]
        [MaxLength(20, ErrorMessage = "Imie nie może być dłuższe niż 20 znaków")]
        [RegularExpression(@"^([A-Za-zzżźćńółęąśŻŹĆĄŚĘŁÓŃ]+)$", ErrorMessage = "Niepoprawne imię")] //TODO: zmienić komunikaty
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [MaxLength(30, ErrorMessage = "Nazwisko nie może być dłuższe niż 30 znaków")]
        [RegularExpression(@"^([A-Za-zzżźćńółęąśŻŹĆĄŚĘŁÓŃ]+)$", ErrorMessage = "Niepoprawna forma nazwiska")]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [MaxLength(40, ErrorMessage = "Email nie może być dłuższy niż 40 znaków")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Niepoprawny adres")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Numer telefonu")]
        
        [MaxLength(12,ErrorMessage = "Numer telefonu nie może być dłuższy niż 12 znaków")] 
        public string Phonenumber { get; set; }
        public bool emailConfirmation { get; set; }

        public DateTime registrationDate { get; set; }


        public virtual Usertype Type { get; set; }
        public virtual Credentials Credentials { get; set; }
        public virtual Useraddress Useraddress { get; set; }
        public virtual ICollection<Message> MessageUserFrom { get; set; }
        public virtual ICollection<Message> MessageUserTo { get; set; }
        public virtual ICollection<Offer> Offer { get; set; }
        public virtual ICollection<Permissionuser> Permissionuser { get; set; }
    }
}
