using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace DbHander
{
    public class ApplicationRole : IdentityRole
    {
        [Required]
        public int Level { get; set; }
    }
}
