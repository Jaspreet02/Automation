using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class SmtpDetail : BaseModel
    {
        public SmtpDetail()
        {

        }
        public int SmtpDetailId { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpUser { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        
        
    }

}
