using System.Collections.Generic;

namespace DbHander
{
   public interface IApplicationRepository : IGenericRepository<Application>
    {
        Dictionary<int,List<int>> ApplicationIdsbyClients(List<int> clientIds);
        string[] CodeExistList(int applicationId);
    }
}
