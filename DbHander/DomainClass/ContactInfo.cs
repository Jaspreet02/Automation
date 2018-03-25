using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class ContactInfo : BaseModel
    {
        public ContactInfo()
        {

        }
        public int ContactInfoId { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }

        

    }
   
}
