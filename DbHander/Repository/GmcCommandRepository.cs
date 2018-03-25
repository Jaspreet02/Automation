using System;
using System.Linq;

namespace DbHander
{
    public class GmcCommandRepository : AbstractRepository, IGmcCommandRepository
    {
        public IQueryable<GMCCommand> FindAll()
        {
                return objDataContext.GmcCommands;
        }
        

        public GMCCommand Find(int id)
        {
                var result = FindAll().Where(x => x.GMCCommandId == id).Select(x => x).SingleOrDefault();
                return result;
        }

        public int  Save(GMCCommand dao)
        {
                GMCCommand dbEntity = new GMCCommand();
                if (dao.GMCCommandId > 0)
                {
                    dbEntity = objDataContext.GmcCommands.Where(x => x.GMCCommandId == dao.GMCCommandId).Select(x => x).SingleOrDefault();
                    dbEntity.ComponentId = dao.ComponentId;
                    dbEntity.ApplicationId = dao.ApplicationId;
                    dbEntity.CommandOrder = dao.CommandOrder;
                    dbEntity.CommandKey = dao.CommandKey;
                    dbEntity.CommandValue = dao.CommandValue;
                    dbEntity.GmcAddonSchenerioId = dao.GmcAddonSchenerioId;
                }
                else
                {
                    dbEntity.ComponentId = dao.ComponentId;
                    dbEntity.ApplicationId = dao.ApplicationId;
                    dbEntity.CommandOrder = dao.CommandOrder;
                    dbEntity.CommandKey = dao.CommandKey;
                    dbEntity.CommandValue = dao.CommandValue;
                    dbEntity.GmcAddonSchenerioId = dao.GmcAddonSchenerioId;
                    objDataContext.GmcCommands.Add(dbEntity);
                }
                objDataContext.SaveChanges();
          
            return dao.GMCCommandId;
        }

        public bool Delete(int id)
        {
                if (id < 1)
                    throw new ArgumentException("Gmc Command Can not be null.");
                var item = objDataContext.GmcCommands.Where(x => x.GMCCommandId == id).Select(x => x).SingleOrDefault();
                
                   objDataContext.GmcCommands.Remove(item);
                    objDataContext.SaveChanges();
                
                return true;
        }

        IQueryable<GMCCommand> IGenericRepository<GMCCommand>.FindAllActive()
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
        // ~GmcCommandRepository() {
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
