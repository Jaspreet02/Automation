using System;
using System.Collections.Generic;
using System.Linq;


namespace DbHander
{
    public class ContactInfoRepository : AbstractRepository, IContactInfoRepository
    {
        public IQueryable<ContactInfo> FindAll()
        {
            return objDataContext.ContactInfos;
        }

        public IQueryable<ContactInfo> FindAllActive()
        {
            return objDataContext.ContactInfos.Where(x => x.Status);
        }

        public ContactInfo Find(int id)
        {
            return objDataContext.ContactInfos.SingleOrDefault(x => x.ContactInfoId.Equals(id));
        }
        
        public List<ContactInfo> GetContactInfoListbyId(string[] ids)
        {    
                int[] intIds = Array.ConvertAll(ids, s => int.Parse(s));
                return GetContactInfoListbyId(intIds);
        }

        public List<ContactInfo> GetContactInfoListbyId(int[] ids)
        {   
                return FindAllActive().Where(x => ids.Contains(x.ContactInfoId)).ToList();
      }

        public int Save(ContactInfo dao)
        {
            ContactInfo entity = objDataContext.ContactInfos.SingleOrDefault(x => x.ContactInfoId.Equals(dao.ContactInfoId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.ContactInfos.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ContactInfoId;
        }

        public bool Delete(int id)
        {
                ContactInfo dbEntity = objDataContext.ContactInfos.Where(x => x.ContactInfoId == id).Select(x => x).SingleOrDefault();
                 objDataContext.ContactInfos.Remove(dbEntity);
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
        // ~ContactInfoRepository() {
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
