using DbHander;
using Microsoft.AspNet.Identity;
using MobileService.Common;
using MobileService.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    public class ComponentController : BaseController
    {
        IComponentRepository _objComponentRepository;
        ITriggerandStatusFileRepository _objTriggerandStatusFileRepository;
        UserManager<ApplicationUser> userManager;

        public ComponentController()
        {
            _objComponentRepository = new ComponentRepository();
            _objTriggerandStatusFileRepository = new TriggerandStatusFilesRepository();
        }

        // GET: api/Client
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(int pageNumber = 0, int pageSize = 10, string sortField = "CreatedAt", string sortOrder = "desc", bool fetchAll = false)
        {
            var result = _objComponentRepository.FindAll().OrderBy(sortField + " " + sortOrder);
            return Ok(await CreatePageResult<Component>(result, pageNumber, pageSize, fetchAll));
        }

        // GET: api/Client/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _objComponentRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Client
        [HttpPost]
        public IHttpActionResult Post(Component value)
        {
            var result = _objComponentRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Client/5
        [HttpPut]
        public IHttpActionResult Put(int id, Component value)
        {
            var result = _objComponentRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<Component> value)
        {
            var result = _objComponentRepository.Find(id);
            value.Patch(result);
            return Ok(_objComponentRepository.Save(result));
        }

        // DELETE: api/Client/5
        public IHttpActionResult Delete(int id)
        {
            return Ok(_objComponentRepository.Delete(id));
        }       
        
        [HttpPost]
        public IHttpActionResult AddUpdateTriggerandStatusFile(TriggerandStatusFile value)
        {
            var result = _objTriggerandStatusFileRepository.Save(value);
            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _objComponentRepository.Dispose();
                _objTriggerandStatusFileRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
