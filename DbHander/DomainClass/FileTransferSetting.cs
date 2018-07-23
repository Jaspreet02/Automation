using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbHander
{
    public class FileTransferSetting : BaseModel
    {
        public FileTransferSetting()
        {

        }
        public int FileTransferSettingId { get; set; }  
        [Required]
        public string Name { get; set; }
        public byte QueueTypeId { get; set; }
        [ForeignKey("QueueTypeId")]
        public QueueType QueueType { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string UserId { get; set; }       
        
    }
}
