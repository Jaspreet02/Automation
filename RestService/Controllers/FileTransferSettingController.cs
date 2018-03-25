using DbHander;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    public class FileTransferSettingController : ApiController
    {
        IFileTransferSettingsRepository _objFileTranserRepository;

        public FileTransferSettingController()
        {
            _objFileTranserRepository = new FileTransferSettingsRepository();
        }

        // GET: api/Client
        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = _objFileTranserRepository.FindAllActive();
            return Ok(result);
        }

        // GET: api/Client/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _objFileTranserRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Client
        [HttpPost]
        public IHttpActionResult Post(FileTransferSetting value)
        {
            var result = _objFileTranserRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Client/5
        [HttpPut]
        public IHttpActionResult Put(int id, FileTransferSetting value)
        {
            var result = _objFileTranserRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<FileTransferSetting> value)
        {
            var result = _objFileTranserRepository.Find(id);
            value.Patch(result);
            return Ok(_objFileTranserRepository.Save(result));
        }

        // DELETE: api/Client/5
        public IHttpActionResult Delete(int id)
        {
            return Ok(_objFileTranserRepository.Delete(id));
        }

        protected override void Dispose(bool disposing)
        {
            _objFileTranserRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
