using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class ProofFile : BaseModel
    {
        public ProofFile()
        {

        }
        public int ProofFileId { get; set; }
        public int ProofId { get; set; }
        public string FileName { get; set; }
        public int UploadStatus { get; set; }
        
        
    }
}
