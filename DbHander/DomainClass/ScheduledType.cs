using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class ScheduledType : BaseModel
    {
        public int Id { get; set; }
        public int AppSchedulerId { get; set; }
        public string Weekly { get; set; }
        public int? Monthly { get; set; }
        public DateTime? Yearly { get; set; }
        
        
        
    }
}
