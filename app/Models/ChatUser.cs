using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class ChatUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public virtual PageUser User { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
