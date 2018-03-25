using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHander
{
   public class RunNumberStatus
    {
        public byte RunNumberStatusId { get; set; }
        public string  Status { get; set; }
    }

    public class ComponentStatus
    {
        public byte ComponentStatusId { get; set; }
        public string Status { get; set; }
    }

    public class JobStatus
    {
        public byte  JobStatusId { get; set; }
        public string Status { get; set; }
    }

    public class QueueType
    {
        public byte QueueTypeId { get; set; }
        public string Status { get; set; }
    }

    public class EmailStatus
    {
        public byte EmailStatusId { get; set; }
        public string Status { get; set; }
    }

    public class EmailToken
    {
        public byte EmailTokenId { get; set; }
        public string Note { get; set; }
        public string Keyword { get; set; }
    }

}
