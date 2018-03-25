using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
   public class RunArchiveDetail : BaseModel
    {
        public int Id { get; set; }
        public string RunNumber { get; set; }
        public int OutputFileArchiveId { get; set; }
        public string InputFile { get; set; }
        public string ArchiveFileName { get; set; }
        public string MoveFileName { get; set; }
        public DateTime ArchiveTime { get; set; }
        public DateTime MoveTime { get; set; }
        
        
    }
}
