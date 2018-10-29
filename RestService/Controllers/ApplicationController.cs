using System.Linq;
using System.Web.Http;
using DbHander;
using System.Web.Http.OData;
using MobileService.Common;
using System.Threading.Tasks;

namespace MobileService.Controllers
{
    [Authorize]
    public class ApplicationController : BaseController
    {
        IApplicationRepository objApplicationRepository;
        IApplicationFilesRepository objApplicationFileRepository;

        public ApplicationController()
        {
            objApplicationRepository = new ApplicationRepository();
            objApplicationFileRepository = new ApplicationFileRepository();
        }

        // GET: api/Application
        [HttpGet]
        public async Task<IHttpActionResult> Get(int pageNumber = 0, int pageSize = 10, string sortField = "CreatedAt", string sortOrder = "desc", bool fetchAll = false)
        {
            var clientList = GenericPrincipalExtensions.Clients(User);
            var result = objApplicationRepository.FindAll().Where(x => clientList.Contains(x.ClientId)).OrderBy(sortField + " " + sortOrder);
            return Ok(await CreatePageResult<Application>(result, pageNumber, pageSize, fetchAll));
        }

        // GET: api/Application/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = objApplicationRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Application
        [HttpPost]
        public IHttpActionResult Post(Application value)
        {
            var result = objApplicationRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Application/5
        [HttpPut]
        public IHttpActionResult Put(int id,Application value)
        {
            var result = objApplicationRepository.Save(value);
            return Ok(result);
        }

        [HttpPatch]
        public IHttpActionResult Patch(int id,Delta<Application> value )
        {
            var result = objApplicationRepository.Find(id);
            value.Patch(result);
            return Ok(objApplicationRepository.Save(result));
        }

        // DELETE: api/Application/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return Ok(objApplicationRepository.Delete(id));
        }


        [HttpGet]
        public IHttpActionResult ApplicationFiles(int applicationId)
        {
            var result = objApplicationFileRepository.GetApplicationFileListByAppID(applicationId);
           return Ok(result);
        }

        // POST: api/Application
        [HttpPost]
        public IHttpActionResult AddApplicationFile(ApplicationFile value)
        {
            var result = objApplicationFileRepository.Save(value);
            return Ok(result);
        }

        // DELETE: api/Application/5
        [HttpDelete]
        public IHttpActionResult DeleteApplicationFile(int applicationFileId)
        {
            return Ok(objApplicationFileRepository.Delete(applicationFileId));
        }

        [HttpGet]
        public IHttpActionResult CodeExistList(int applicationId)
        {
            return Ok(objApplicationRepository.CodeExistList(applicationId));
        }

        protected override void Dispose(bool disposing)
        {
            objApplicationRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
