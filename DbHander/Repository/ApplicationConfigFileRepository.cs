﻿using System;
using System.Linq;

namespace DbHander
{
    public class ApplicationConfigFileRepository : AbstractRepository, IApplicationConfigFileRepository
    {

        public IQueryable<ApplicationConfigFile> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ApplicationConfigFile> FindAllActive()
        {
            throw new NotImplementedException();
        }

        public ApplicationConfigFile Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Save(ApplicationConfigFile dao)
        {
                ApplicationConfigFile entity = objDataContext.ApplicationConfigFiles.SingleOrDefault(x => x.ApplicationConfigFileId.Equals(dao.ApplicationConfigFileId));
                if (entity != null)
                {
                    dao.ModifiedAt = DateTimeOffset.Now;
                    objDataContext.Entry(entity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    dao.ModifiedAt = dao.CreatedAt;
                    objDataContext.ApplicationConfigFiles.Add(dao);
                }
                objDataContext.SaveChanges();
                return dao.ApplicationConfigFileId;
        }

        public bool Delete(int id)
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
        // ~ApplicationConfigFileRepository() {
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
