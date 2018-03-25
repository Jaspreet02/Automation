using System;
using System.Linq;

namespace DbHander
{
  public interface IGenericRepository<T> : IDisposable
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindAllActive();
        T Find(int id);
        int Save(T dao);
        bool Delete(int id);
        bool UpdateStatus(int id);
    }
}
