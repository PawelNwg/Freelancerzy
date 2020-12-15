using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class Mail
    {
        private String smtpEmailAdress = "freeelancerzy@gmail.com";
        public String SmtpEmailAdress { get { return smtpEmailAdress; } }

        private String emailSubject = "potwierdzenie rejestracji";
        public String EmailSubject { get { return emailSubject; } }

        private String emailBody = "";
        public String EmailBody { get { return emailBody; }  }

        private String smtpHost = "smtp.gmail.com";
        public String SmtpHost { get { return smtpHost; }  }

        private int smtpPort = 587;
        public int SmtpPort { get { return smtpPort; } }
    }
}
