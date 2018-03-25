using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class ClientSmtp : BaseModel
    {
        public ClientSmtp()
        {

        }
        public int ClientSmtpId { get; set; }
        public int ClientId { get; set; }
        public int SmtpId { get; set; }
        
        

    }
}
