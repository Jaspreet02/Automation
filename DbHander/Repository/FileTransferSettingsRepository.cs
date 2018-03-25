using System;
using System.Linq;

namespace DbHander
{
    public class FileTransferSettingsRepository : AbstractRepository, IFileTransferSettingsRepository
    { 
        public IQueryable<FileTransferSetting> FindAll()
        {
            return objDataContext.FileTransferSettings;
        }

        public IQueryable<FileTransferSetting> FindAllActive()
        {
            return objDataContext.FileTransferSettings.Where(x => x.Status);
        }

        public FileTransferSetting Find(int id)
        {
            return objDataContext.FileTransferSettings.SingleOrDefault(x => x.FileTransferSettingId.Equals(id));
        }

        public int Save(FileTransferSetting dao)
        {
            FileTransferSetting entity = objDataContext.FileTransferSettings.SingleOrDefault(x => x.FileTransferSettingId.Equals(dao.FileTransferSettingId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.FileTransferSettings.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.FileTransferSettingId;
        }
      
        public bool Delete(int id)
        {
                var result = objDataContext.FileTransferSettings.SingleOrDefault(x => x.FileTransferSettingId == id);
                objDataContext.FileTransferSettings.Remove(result);
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
        // ~FileTransferSettingsRepository() {
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
