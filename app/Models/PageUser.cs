using System;
using System.Collections.Generic;

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
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public int? Phonenumber { get; set; }

        public virtual Usertype Type { get; set; }
        public virtual Credentials Credentials { get; set; }
        public virtual Useraddress Useraddress { get; set; }
        public virtual ICollection<Message> MessageUserFrom { get; set; }
        public virtual ICollection<Message> MessageUserTo { get; set; }
        public virtual ICollection<Offer> Offer { get; set; }
        public virtual ICollection<Permissionuser> Permissionuser { get; set; }
    }
}
