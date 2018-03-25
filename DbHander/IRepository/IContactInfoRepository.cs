using System.Collections.Generic;

namespace DbHander
{
   public interface IContactInfoRepository : IGenericRepository<ContactInfo>
    {
       List<ContactInfo> GetContactInfoListbyId(string[] ids);
       List<ContactInfo> GetContactInfoListbyId(int[] ids);
    }
}
