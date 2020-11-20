using System;
using System.Collections.Generic;

namespace freelancerzy.Models
{
    public partial class Useraddress
    {
        public int Userid { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public int? ApartmentNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

        public virtual PageUser User { get; set; }
    }
}
