using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace freelancerzy.Models
{
    public partial class Message
    {
        public Message()
        {
            Messagereport = new HashSet<Messagereport>();
        }

        public int Messageid { get; set; }
        public int UserFromId { get; set; }
        public int UserToId { get; set; }
        public DateTime Date { get; set; }
        [Display(Name="Treść")]
        [Required(ErrorMessage ="Nie możesz wysłać wiadomości bez treści")]
        [MaxLength(250,ErrorMessage ="Pojedyńcza wiadomość może mieć 250 znaków")]
        public string Content { get; set; }
        public string Status { get; set; }
        
        public bool Seen { get; set; }
        public int ChatId { get; set; }
        public virtual PageUser UserFrom { get; set; }
        public virtual PageUser UserTo { get; set; }
        public virtual ICollection<Messagereport> Messagereport { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
