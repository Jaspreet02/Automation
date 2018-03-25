using System.Linq;

namespace DbHander
{
   public interface IComponentOutputLocationRepository : IGenericRepository<ComponentOutputLocation>
    {
        IQueryable<ComponentOutputLocation> GetOutputLocations(int appId, int compId);
    }
}
