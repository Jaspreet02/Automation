using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbHander
{
    public class RunDetail : BaseModel
    {
        public RunDetail()
        {

        }
        public int RunDetailId { get; set; }
        [Required]
        public string RunNumber { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public byte RunNumberStatusId { get; set; }
        [ForeignKey("RunNumberStatusId")]
        public RunNumberStatus RunNumberStatus { get; set; }
        public virtual ICollection<RunComponentStatus> RunComponentStatus { get; set; }
    }
}
