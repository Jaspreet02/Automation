using System.ComponentModel.DataAnnotations.Schema;

namespace DbHander
{
    public class ApplicationConfigFile : BaseModel
    {
        public ApplicationConfigFile()
        {

        }
        public int ApplicationConfigFileId { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public int PDFPage { get; set; }
        
        
        
    }
}
