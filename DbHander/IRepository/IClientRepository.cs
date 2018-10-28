using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        IQueryable<int> ClientIdsbyUser(List<string> userIds);
        string[] CodeExistList(int clientId);
    }
}
