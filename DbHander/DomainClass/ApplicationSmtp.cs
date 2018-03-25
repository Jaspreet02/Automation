using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
   public class ApplicationSmtp : BaseModel
    {
        public ApplicationSmtp()
        {

        }
        public int ApplicationSmtpId { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public int SmtpId { get; set; }
        
        
    }
}
