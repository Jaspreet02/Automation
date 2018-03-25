using DbHander;
using MobileService.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    public class FileTransferController : BaseController
    {
        IFileTransferSettingsRepository objFileTransferRepository;

        public FileTransferController()
        {
            objFileTransferRepository = new FileTransferSettingsRepository();
        }

        // GET: api/Application
        [HttpGet]
        public async Task<IHttpActionResult> Get(int pageNumber = 1, int pageSize = 10, bool fetchAll = false)
        {
            var result = objFileTransferRepository.FindAll().AsQueryable().OrderByDescending(x=> x.CreatedAt);
            return Ok(await CreatePageResult<FileTransferSetting>(result, pageNumber, pageSize,fetchAll));
        }

        // GET: api/Application/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = objFileTransferRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Application
        [HttpPost]
        public IHttpActionResult Post(FileTransferSetting value)
        {
            var result = objFileTransferRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Application/5
        [HttpPut]
        public IHttpActionResult Put(int id, FileTransferSetting value)
        {
            value.FileTransferSettingId = id;
           var result = objFileTransferRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<FileTransferSetting> value)
        {
            var result = objFileTransferRepository.Find(id);
            value.Patch(result);
            return Ok(objFileTransferRepository.Save(result));
        }

        // DELETE: api/Application/5
        public IHttpActionResult Delete(int id)
        {
            return Ok(objFileTransferRepository.Delete(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                objFileTransferRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
