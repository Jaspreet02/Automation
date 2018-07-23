using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class Client : BaseModel
    {
        public Client()
        {

        }
        public int ClientId { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [StringLength(3)]
        public string Code { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Not a valid email address")]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(15)]
        public string Contact { get; set; }
        [StringLength(3)]
        public string ProofFormat { get; set; }
        public string ProofPassword { get; set; }
        public string ProofName { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int ProofsAge { get; set; }      
        
    }
}
