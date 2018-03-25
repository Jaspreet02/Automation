using System.Linq;

namespace DbHander
{
    /// <summary>
    /// Application Component
    /// </summary>
    public interface IComponentInputLocationRepository : IGenericRepository<ComponentInputLocation>
    {
        IQueryable<ComponentInputLocation> GetInputLocations(int appId, int componentId);
    }
}
