using System;
using System.Linq;

namespace DbHander
{
    public  class ApplicationSmtpRepository : AbstractRepository, IApplicationSmtpRepository
    {
        public IQueryable<ApplicationSmtp> FindAll()
        {
                var list = objDataContext.ApplicationSmtps;
                return list;
        }

        public IQueryable<ApplicationSmtp> FindAllActive()
        {
            return objDataContext.ApplicationSmtps;
        }

        public ApplicationSmtp Find(int id)
        {
                return objDataContext.ApplicationSmtps.SingleOrDefault(x => x.ApplicationSmtpId.Equals(id));
           }

        public int Save(ApplicationSmtp dao)
        {
                ApplicationSmtp entity = objDataContext.ApplicationSmtps.SingleOrDefault(x => x.ApplicationSmtpId.Equals(dao.ApplicationSmtpId));
                if (entity != null)
                {
                    dao.ModifiedAt = DateTimeOffset.Now;
                    objDataContext.Entry(entity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    dao.ModifiedAt = dao.CreatedAt;
                    objDataContext.ApplicationSmtps.Add(dao);
                }
                objDataContext.SaveChanges();
            
            return dao.ApplicationSmtpId;
        }

        public bool Delete(int id)
        {
                var Smtp = objDataContext.ApplicationSmtps.Where(x => x.ApplicationSmtpId == id).Select(x => x).SingleOrDefault();
                               
                    objDataContext.ApplicationSmtps.Remove(Smtp);
                    objDataContext.SaveChanges();
               
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
        // ~ApplicationSmtpRepository() {
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
