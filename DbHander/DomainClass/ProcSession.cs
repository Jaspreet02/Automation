using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
   public class ProcSession : BaseModel
    {
        public ProcSession()
        {

        }
        public int ProcSessionId { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public string ProcessID { get; set; }
        public int ProcComponentID { get; set; }
        public string SessionKey { get; set; }
        public byte ProcStatus { get; set; }
        [ForeignKey("ProcStatus")]
        public JobStatus JobStatus { get; set; }
        public bool KillRequired { get; set; }
        public DateTime? ExpectedDateTime { get; set; }
    }
}
