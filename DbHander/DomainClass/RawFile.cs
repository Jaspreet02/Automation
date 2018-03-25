using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
   public class RawFile : BaseModel
    {       
        public RawFile()
        {

        }
       public int RawFileId { get; set; }
       public int RunNumberId { get; set; }
        [ForeignKey("RunNumberId")]
        public RunDetail RunNumber { get; set; }
        public string FileName { get; set; }
       public string HotFolder { get; set; }                
    }
}
