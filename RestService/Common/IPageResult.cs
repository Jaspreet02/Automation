using System.Linq;

namespace MobileService
{
    public interface IPageResult<T>
    {
        int Count { get; set; }
        IQueryable<T> Result { get; set; }
    }
}
