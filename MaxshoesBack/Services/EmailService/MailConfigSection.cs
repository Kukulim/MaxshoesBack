using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxshoesBack.Services.EmailService
{
    public class MailConfigSection
    {
        public string From { get; set; }

        public string MailgunKey { get; set; }

        public string Domain { get; set; }
    }
}
