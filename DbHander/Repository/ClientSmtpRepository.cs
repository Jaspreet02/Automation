using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public class ClientSmtpRepository : AbstractRepository
    {

        public IEnumerable<ClientSmtp> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ClientSmtp> FindAllActive()
        {
            throw new NotImplementedException();
        }

        public ClientSmtp Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Save(ClientSmtp dao)
        {
            return dao.ClientSmtpId;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool ChangeStatus(int id, bool status)
        {
            throw new NotImplementedException();
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
        // ~ClientSmtpRepository() {
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
