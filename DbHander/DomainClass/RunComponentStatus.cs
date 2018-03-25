using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbHander
{
    public class RunComponentStatus : BaseModel
    {
        public RunComponentStatus()
        {

        }
        public int RunComponentStatusId { get; set; }
        public int RunNumberId { get; set; }
        [ForeignKey("RunNumberId")]
        public RunDetail RunDetail { get; set; }
        public int ComponentId { get; set; }
        [ForeignKey("ComponentId")]
        public Component Component { get; set; }
        public int ComponentOrder { get; set; }
        public byte ComponentStatusId { get; set; }   
        [ForeignKey("ComponentStatusId")]
        public ComponentStatus ComponentStatus { get; set; }     
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string Message { get; set; }
    }
}
