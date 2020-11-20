using System;
using System.Collections.Generic;

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
        public string Content { get; set; }
        public string Status { get; set; }

        public virtual PageUser UserFrom { get; set; }
        public virtual PageUser UserTo { get; set; }
        public virtual ICollection<Messagereport> Messagereport { get; set; }
    }
}
