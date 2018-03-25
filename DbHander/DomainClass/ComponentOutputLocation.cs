using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class ComponentOutputLocation : BaseModel
    {
        public ComponentOutputLocation()
        {

        }
        public int ComponentOutputLocationId { get; set; }
        public int ComponentId { get; set; }
        [ForeignKey("ComponentId")]
        public Component     Component { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        [Required]
        public string TagName { get; set; }
        [Required]
        public string FileMask { get; set; }
        [Required]
        public string OutputLocation { get; set; }
        
        
        
    }
}
