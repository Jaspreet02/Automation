using System;
using System.Linq;

namespace DbHander
{
    public class ComponentRepository : AbstractRepository, IComponentRepository
    {
        public int Save(Component dao)
        {
            Component entity = objDataContext.Components.SingleOrDefault(x => x.ComponentId.Equals(dao.ComponentId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.Components.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ComponentId;
        }
        
        public IQueryable<Component> FindAll()
        {
            return objDataContext.Components.Include("TriggerandStatusFile");
        }

        public IQueryable<Component> FindAllActive()
        {
            return objDataContext
                        .Components
                        .Where(x => (x.Status));
        }

        public Component Find(int id)
        {
            return objDataContext.Components.SingleOrDefault(x => x.ComponentId.Equals(id));
        }

        public bool Delete(int id)
        {
            if (id < 1)
                throw new ArgumentException("component Can not be null.");

            var compTrigfiles = objDataContext.TriggerandStatusFiles.Where(x => x.ComponentId == id).SingleOrDefault();

            if (compTrigfiles != null)
            {
                objDataContext.TriggerandStatusFiles.Remove(compTrigfiles);
            }


            var component = objDataContext.Components.Where(x => x.ComponentId == id).Select(x => x).SingleOrDefault();
            // var Client = Mapper.Mapper.Map(Find(ID));

            objDataContext.Components.Remove(component);
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
        // ~ComponentRepository() {
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
