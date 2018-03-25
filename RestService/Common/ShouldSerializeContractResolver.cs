using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MobileService.Common
{
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        IList<string> attributesList = new List<string>();

       public ShouldSerializeContractResolver(IList<string> itemList)
        {
            attributesList = itemList;
        }
 
   protected override IList<JsonProperty> CreateProperties(Type objType, MemberSerialization memberSerialization)
     {
       IList<JsonProperty> properties = base.CreateProperties(objType, memberSerialization);
            return properties.Where(p => attributesList.Contains(p.PropertyName)).ToList();
        }
    }
}