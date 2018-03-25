using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class UploadFile : BaseModel
    {
        public UploadFile()
        {

        }
        public int UploadFileId { get; set; }
        [Required]
        public string Name { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public int ComponentId { get; set; }
        [ForeignKey("ComponentId")]
        public Component Component { get; set; }
        [Required]
        public string FileInputPath { get; set; }
        [Required]
        public string InputFileMask { get; set; }
        public bool IsArchiveOutputRequired { get; set; }
        public string ArchiveOutputPath { get; set; }
        public int ArchiveFileTransferSettingId { get; set; }
        public bool IsMoveFileRequired { get; set; }
        public string MoveFileExpression { get; set; }
        public string ArchiveFileExpression { get; set; }
        public string MoveFilePath { get; set; }
        public int MoveFileTransferSettingId { get; set; }
        public string Description { get; set; }       
    }
}
