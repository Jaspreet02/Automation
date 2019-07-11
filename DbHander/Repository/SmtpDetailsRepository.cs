using System;
using System.Linq;

namespace DbHander
{
    public class SmtpDetailsRepository : AbstractRepository, ISmtpDetailsRepository
    {        
        public IQueryable<SmtpDetail> FindAll()
        {
            return objDataContext.SmtpDetails;
        }

        public SmtpDetail Find(int id)
        {
            return objDataContext.SmtpDetails.SingleOrDefault(x => x.SmtpDetailId.Equals(id));
        }

        public int Save(SmtpDetail dao)
        {
            SmtpDetail entity = objDataContext.SmtpDetails.SingleOrDefault(x => x.SmtpDetailId.Equals(dao.SmtpDetailId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.SmtpDetails.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.SmtpDetailId;
        }

        public bool Delete(int id)
        {
                SmtpDetail dbEntity = objDataContext.SmtpDetails.Where(x => x.SmtpDetailId == id).Select(x => x).SingleOrDefault();
                objDataContext.SmtpDetails.Remove(dbEntity);
                objDataContext.SaveChanges();
                return true;
        }

        public IQueryable<SmtpDetail> FindAllActive()
        {
            return objDataContext.SmtpDetails;
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
        // ~SmtpDetailsRepository() {
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
