using System;
using System.Linq;

namespace DbHander
{
    public class ComponentConfigRepository : AbstractRepository, IComponentConfigRepository
    {
        public IQueryable<ComponentConfig> FindAll()
        {
                var result = objDataContext.ComponentConfigs;
                return result;
         }

        public IQueryable<ComponentConfig> FindAllActive()
        {
                var result = objDataContext.ComponentConfigs.Where(x => x.Status == true);
                return result;
        }

        public ComponentConfig Find(int id)
        {
                return objDataContext.ComponentConfigs.SingleOrDefault(x => x.ComponentConfigId.Equals(id));
        }

        public int Save(ComponentConfig dao)
        {
                ComponentConfig entity = objDataContext.ComponentConfigs.SingleOrDefault(x => x.ComponentConfigId.Equals(dao.ComponentConfigId));
                if (entity != null)
                {
                    dao.ModifiedAt = DateTimeOffset.Now;
                    objDataContext.Entry(entity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    dao.ModifiedAt = dao.CreatedAt;
                    objDataContext.ComponentConfigs.Add(dao);
                }
                objDataContext.SaveChanges();
                return dao.ComponentConfigId;
          }

        public bool Delete(int id)
        {
                var ConfigFile = objDataContext.ComponentConfigs.Where(x => x.ComponentConfigId == id).Select(x => x).FirstOrDefault();
                objDataContext.ComponentConfigs.Remove(ConfigFile);
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
        // ~ComponentConfigRepository() {
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
