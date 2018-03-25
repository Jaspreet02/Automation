using System;
using System.Linq;

namespace DbHander
{
    public class ComponentOutputLocationRepository : AbstractRepository, IComponentOutputLocationRepository
    {
        public IQueryable<ComponentOutputLocation> GetOutputLocations(int appId, int compId)
        {
            return objDataContext.ComponentOutputLocations.Where(x => x.ApplicationId == appId && x.ComponentId == compId && x.Status == true);
        }

        public bool Delete(int id)
        {
            var entity = objDataContext.ComponentOutputLocations.SingleOrDefault(x => x.ComponentOutputLocationId == id);
            if (entity != null)
            {
                objDataContext.ComponentOutputLocations.Remove(entity);
                objDataContext.SaveChanges();
            }
            return true;
        }

        public ComponentOutputLocation Find(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ComponentOutputLocation> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ComponentOutputLocation> FindAllActive()
        {
            throw new NotImplementedException();
        }

        public int Save(ComponentOutputLocation dao)
        {
            ComponentOutputLocation entity = objDataContext.ComponentOutputLocations.SingleOrDefault(x => x.ComponentOutputLocationId.Equals(dao.ComponentOutputLocationId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                dao.TagName = "OutputLocation";
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                dao.TagName = "OutputLocation";
                objDataContext.ComponentOutputLocations.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ComponentOutputLocationId;
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
        // ~ComponentOutputLocationRepository() {
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
