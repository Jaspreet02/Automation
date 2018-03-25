using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class TriggerandStatusFile : BaseModel
    {
        public TriggerandStatusFile()
        {

        }
            
        [Key,ForeignKey("Component")]
        public int ComponentId { get; set; }
        public virtual Component Component { get; set; }
        public string TriggerFilelocation { get; set; }
        public string StepStatusLocation { get; set; }


    }
}
