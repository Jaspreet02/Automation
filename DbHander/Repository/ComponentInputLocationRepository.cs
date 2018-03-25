using System;
using System.Linq;

namespace DbHander
{
    public class ComponentInputLocationRepository : AbstractRepository, IComponentInputLocationRepository
    {
        public bool Delete(int id)
        {
            var entity = objDataContext.ComponentInputLocations.SingleOrDefault(x => x.ComponentInputLocationId == id);
            if (entity != null)
            {
                objDataContext.ComponentInputLocations.Remove(entity);
                objDataContext.SaveChanges();
            }
            return true;
        }

        public ComponentInputLocation Find(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ComponentInputLocation> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ComponentInputLocation> FindAllActive()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ComponentInputLocation> GetInputLocations(int appId, int componentId)
        {
            return objDataContext.ComponentInputLocations.Where(x => x.ApplicationId == appId && x.ComponentId == componentId && x.Status == true);
        }
        
        public int Save(ComponentInputLocation dao)
        {
            ComponentInputLocation entity = objDataContext.ComponentInputLocations.SingleOrDefault(x => x.ComponentInputLocationId.Equals(dao.ComponentInputLocationId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                dao.TagName = "InputLocation";
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                dao.TagName = "InputLocation";
                objDataContext.ComponentInputLocations.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ComponentInputLocationId;
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
        // ~ComponentInputLocationRepository() {
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
