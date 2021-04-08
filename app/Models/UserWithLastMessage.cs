using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    [NotMapped]
    public class UserWithLastMessage
    {
        public PageUser User { get; set; }
        public Message LastMessage { get; set; }
        public int ChatId { get; set; }
    }
}
