using System;
using System.Collections.Generic;
using System.Linq;


namespace DbHander
{
    public class EmailTrackingRepository : AbstractRepository, IEmailTrackingRepository
    {
        public IQueryable<EmailTracking> FindAll()
        {
                var EmailTrackings = objDataContext.EmailTrackings;
                return EmailTrackings;
        }

        public IQueryable<EmailTracking> FindAllActive()
        {
            return objDataContext.EmailTrackings;
        }

        public EmailTracking Find(int id)
        {
                var result = objDataContext.EmailTrackings.SingleOrDefault(x => x.EmailTrackingId.Equals(id));
                return result;  }

        public int Save(EmailTracking dao)
        {
                EmailTracking entity = objDataContext.EmailTrackings.SingleOrDefault(x => x.EmailTrackingId.Equals(dao.EmailTrackingId));
                if(entity != null)
                {
                    objDataContext.Entry(entity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    objDataContext.EmailTrackings.Add(entity);
                }
                objDataContext.SaveChanges();
           
            return dao.EmailTrackingId;
        }

        public bool Delete(int id)
        {
                EmailTracking dbEntity = objDataContext.EmailTrackings.Where(x => x.EmailTrackingId == id).FirstOrDefault();
                if(dbEntity != null)
                {
                    objDataContext.EmailTrackings.Remove(dbEntity);
                    objDataContext.SaveChanges();
                }
                return true;
         }

       public IEnumerable<EmailTracking> GetEmailTrackingByKeyword(int runNumberId)
        {
                var query = FindAll();

                if(runNumberId > 0)
                    query = query.Where(ET => ET.RunNumberId == runNumberId);
                else
                    return new List<EmailTracking>();

                return query.Select(x => x).ToList();
        }

        public IQueryable<EmailTracking> GetReadyEmails(int status)
        {
                return objDataContext.EmailTrackings.Where(x=> x.EmailStatus.Equals(status));
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
        // ~EmailTrackingRepository() {
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
