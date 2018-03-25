using System.Linq;

namespace MobileService
{
    public class PageResult<T> : IPageResult<T>
    {
       public int Count { get; set; }
       public IQueryable<T> Result { get; set; }
    }
}