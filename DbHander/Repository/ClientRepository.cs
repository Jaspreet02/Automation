using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace DbHander
{
    public class ClientRepository : AbstractRepository, IClientRepository
    {

        public IQueryable<int> ClientIdsbyUser(List<string> userIds)
        {
            return objDataContext.Clients.Where(x => userIds.Contains(x.UserId) && x.Status == true).Select(x => x.ClientId);
        }

        public IQueryable<Client> FindAll()
        {
            return objDataContext.Clients;
          }

        public IQueryable<Client> FindAllActive()
        {
                return objDataContext
                            .Clients
                            .Where(x => (x.Status == true));
            }

        
        public Client Find(int id)
        {
                return objDataContext.Clients.SingleOrDefault(x => x.ClientId.Equals(id));
         }

        public int Save(Client dao)
        {
            Client entity = objDataContext.Clients.SingleOrDefault(x => x.ClientId.Equals(dao.ClientId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.Clients.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ClientId;
        }
        
        public bool Delete(int id)
        {
            //Execute stored procedure as a function          
            SqlParameter param = new SqlParameter("@clientId", id);
            var result = objDataContext.Database.ExecuteSqlCommand("EXEC DeleteClient @clientId", param);
            return true;
        }
        
        public bool UpdateStatus(int id)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    objDataContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ClientRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

    }
}
