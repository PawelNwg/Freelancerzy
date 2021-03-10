using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace freelancerzy.Models
{
    public class MailPasswd
    {
        private String smtpEmailAdress = "freeelancerzy@gmail.com";
        public String SmtpEmailAdress { get { return smtpEmailAdress; } }

        private String emailSubject = "Freelancerzy - nowe hasło";
        public String EmailSubject { get { return emailSubject; } }

        private String smtpHost = "smtp.gmail.com";
        public String SmtpHost { get { return smtpHost; } }

        private int smtpPort = 587;
        public int SmtpPort { get { return smtpPort; } }

        public String EmailBody { get { return emailBody; } }
        private String emailBody = File.ReadAllText("Data/mailBodyPsswd.html");
    }
}
