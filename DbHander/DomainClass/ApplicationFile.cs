using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbHander
{
    public class ApplicationFile : BaseModel
    {
        public ApplicationFile()
        {

        }
        public int ApplicationFileId { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        [Required]
        public string Mask { get; set; }
        public bool IsRequired { get; set; }  
        public int ProofRestrictedDays { get; set; }        
        
    }
}
