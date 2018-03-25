using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class Application : BaseModel
    {
        public  Application()
        {

        }
        public int ApplicationId { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Use 1-30 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(3)]
        public string Code { get; set; }
        public int ProcessingType { get; set; }
        public bool IsBatch { get; set; }
        public int SLA { get; set; }
        public int FileTransferSettingId { get; set; }
        [ForeignKey("FileTransferSettingId")]
        public FileTransferSetting FileTransferSetting { get; set; }
        [Required]
        public string HotFolder { get; set; }
        public string ArchivePath { get; set; }
        public string ArchiveFileName { get; set; }
        public bool IsArchive { get; set; }
        public bool IsFileMove { get; set; }   
        [Required]
        public string InputPath { get; set; }
        public int ArchivalDays { get; set; }
    }
}
