﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace DbHander
{
    public class EmailTrackingRepository : AbstractRepository, IEmailTrackingRepository
    {
        public IQueryable<EmailTracking> FindAll()
        {
            return objDataContext.EmailTrackings;
        }

        public IQueryable<EmailTracking> FindAllActive()
        {
            return objDataContext.EmailTrackings.Where(x=> x.Status);
        }

        public EmailTracking Find(int id)
        {
            var result = objDataContext.EmailTrackings.SingleOrDefault(x => x.EmailTrackingId.Equals(id));
            return result;
        }

        public int Save(EmailTracking dao)
        {
            dao.FromEmailId = "1";
            EmailTracking entity = objDataContext.EmailTrackings.SingleOrDefault(x => x.EmailTrackingId.Equals(dao.EmailTrackingId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.EmailTrackings.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.EmailTrackingId;
        }

        public bool Delete(int id)
        {
            EmailTracking dbEntity = objDataContext.EmailTrackings.Where(x => x.EmailTrackingId == id).FirstOrDefault();
            if (dbEntity != null)
            {
                objDataContext.EmailTrackings.Remove(dbEntity);
                objDataContext.SaveChanges();
            }
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
