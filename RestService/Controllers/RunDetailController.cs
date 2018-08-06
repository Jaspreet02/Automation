using System;
using System.Web.Http;
using MobileService.Common;
using System.Collections.Generic;
using DbHander;
using System.Linq;
using System.Threading.Tasks;

namespace MobileService.Controllers
{
    [Authorize]
    public class RunDetailController : BaseController
    {
        IRunDetailsRepository _objRunDetailRepository;

        public RunDetailController()
        {
            _objRunDetailRepository = new RunDetailsRepository();
        }

        // GET: api/Client
        [HttpGet]
        public async Task<IHttpActionResult> Get(int clientId, int appId, int status, int pageNumber = 1, int pageSize = 10, bool fetchAll = false)
        {
            var appIds = new List<int>();
            if (clientId == 0)
                appIds = GenericPrincipalExtensions.Applications(User, null);
            else
            {
                if (appId != 0)
                    appIds.Add(appId);
                else
                    appIds = GenericPrincipalExtensions.Applications(User, clientId);
            }

            var result = _objRunDetailRepository.FindAllActive().Where(x=> appIds.Contains(x.ApplicationId) && (x.RunNumberStatusId == status || status == -1)).OrderByDescending(x => x.CreatedAt);
            return Ok(await CreatePageResult<RunDetail>(result,pageNumber,pageSize, fetchAll));
        }

        [HttpGet]
        public IHttpActionResult RunStatus()
        {
            return Ok(Enum.GetNames(typeof(RunNumberStatusType)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _objRunDetailRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
