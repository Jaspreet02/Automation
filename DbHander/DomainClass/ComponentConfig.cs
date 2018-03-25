using System.ComponentModel.DataAnnotations.Schema;

namespace DbHander
{
    public class ComponentConfig : BaseModel
    {
        public ComponentConfig() { }
       public int ComponentConfigId { get; set; }
       public int ApplicationComponentId { get; set; }
        [ForeignKey("ApplicationComponentId")]
        public ApplicationComponent ApplicationComponent { get; set; }
        public string ConfigFile { get; set; }
       public string ConfigName { get; set; }
        
        
    }
}
