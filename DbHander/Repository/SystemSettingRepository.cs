using System;
using System.Linq;

namespace DbHander
{
    public class SystemSettingRepository : AbstractRepository, ISystemSettingRepository
    {
        public IQueryable<SystemSetting> FindAll()
        {
            return objDataContext.SystemSettings;
        }

        public IQueryable<SystemSetting> FindAllActive()
        {
            return objDataContext.SystemSettings.Where(x => x.Status);
        }

        public SystemSetting Find(int id)
        {
            throw new NotImplementedException();
        }

        public int Save(SystemSetting dao)
        {
            return dao.SystemSettingId;
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
        // ~SystemSettingRepository() {
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
