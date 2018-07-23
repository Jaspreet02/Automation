using DbHander;
using Microsoft.AspNet.Identity.Owin;
using MobileService.App_Start;
using MobileService.Common;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MobileService.Controllers
{
    public class BaseController : ApiController
    {
        private ApplicationUserManager _userManager;
        
        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
            Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); 
            }
            private set
            {
                _userManager = value;
            }
        }

        protected async Task<IPageResult<T>> CreatePageResult<T>(IQueryable<T> items, int pageNumber, int pageSize,bool fetchAll)
        {
            pageNumber = fetchAll ? 0 : pageNumber;
            var count = await items.CountAsync();
            pageSize = fetchAll ? count : pageSize;
            var result = items.Skip(pageNumber * pageSize).Take(pageSize);
            return new PageResult<T>()
            {
                Result = result,
                Count = count
            };
        }
    }
}
