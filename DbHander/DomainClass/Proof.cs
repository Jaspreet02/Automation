using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
   public class Proof : BaseModel
    {
        public Proof()
        {

        }

       public int ProofId { get; set; }
       public int ClientId { get; set; }
       public int ApplicationId { get; set; }
       public int RunNumberId { get; set; }
       public String InputFileName { get; set; }
       public DateTime CreatedDate { get; set; }
        
        public DateTime? SendDateTime { get; set; }
       public DateTime? ApproveRejectDate { get; set; }
       public int? ApproveRejectBy { get; set; }
       public char? ProofStatus { get; set; }
       public String Notes { get; set; }
             
    }
}
