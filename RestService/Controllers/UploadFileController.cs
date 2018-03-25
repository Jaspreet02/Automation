using System.Web.Http;
using System.Web.Http.OData;
using DbHander;
using System.Linq;
using System.Threading.Tasks;
using MobileService.Common;

namespace MobileService.Controllers
{
    public class UploadFileController : BaseController
    {
        IUploadFileRepository objUploadFileRepository;

        public UploadFileController()
        {
            objUploadFileRepository = new UploadFileRepository();
        }

        // GET: api/Application
        [HttpGet]
        public async Task<IHttpActionResult> Get(int pageNumber = 1, int pageSize = 10, bool fetchAll = false)
        {
            var appIds = GenericPrincipalExtensions.Applications(User, null);
            var result = objUploadFileRepository.FindAll().Where(x=> appIds.Contains(x.ApplicationId)).OrderByDescending(x => x.CreatedAt);
            return Ok(await CreatePageResult<UploadFile>(result, pageNumber, pageSize, fetchAll));
        }

        // GET: api/Application/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = objUploadFileRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Application
        [HttpPost]
        public IHttpActionResult Post(UploadFile value)
        {
            var result = objUploadFileRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Application/5
        [HttpPut]
        public IHttpActionResult Put(int id, UploadFile value)
        {
            value.UploadFileId = id;
            var result = objUploadFileRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<UploadFile> value)
        {
            var result = objUploadFileRepository.Find(id);
            value.Patch(result);
            return Ok(objUploadFileRepository.Save(result));
        }

        // DELETE: api/Application/5
        public IHttpActionResult Delete(int id)
        {
            return Ok(objUploadFileRepository.Delete(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                objUploadFileRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
