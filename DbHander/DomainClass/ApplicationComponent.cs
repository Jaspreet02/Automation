using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class ApplicationComponent : BaseModel
    {
        public ApplicationComponent()
        {

        }
        public int ApplicationComponentId { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public int ComponentId { get; set; }
        [ForeignKey("ComponentId")]
        public Component Component { get; set; }
        [Required]
        public int ComponentOrder { get; set; } 
        public bool IsOptional { get; set; }
    }
}
