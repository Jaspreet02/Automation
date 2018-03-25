using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DbHander
{
   public class Component : BaseModel
    {
        public Component()
        {
        }
        public int ComponentId { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        public string ComponentExe { get; set; }
        public string Detail { get; set; }
        [Required]
        [StringLength(3)]
        public string ShortName { get; set; }        
        public bool IsOptional { get; set; }
        public virtual TriggerandStatusFile TriggerandStatusFile { get; set; }

    }
}
