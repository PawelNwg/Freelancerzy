using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class Chat
    {
        public Chat()
        {
            ChatUsers = new HashSet<ChatUser>();
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
       
        public virtual ICollection<ChatUser> ChatUsers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
       
        

    }
}
