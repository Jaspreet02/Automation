using DbHander;
using MobileService.Common;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    public class AppComponentController : ApiController
    {
        IApplicationComponentRepository _objAppComponentRepository;
        IComponentInputLocationRepository _objInputLocationRepository;
        IComponentOutputLocationRepository _objOutputLocationRepository;

        public AppComponentController()
        {
            _objAppComponentRepository = new ApplicationComponentRepository();
            _objInputLocationRepository = new ComponentInputLocationRepository();
            _objOutputLocationRepository = new ComponentOutputLocationRepository();
        }

        // GET: api/Application
        [HttpGet]
        public IHttpActionResult GetApplication(int applicationId)
        {
            var result = _objAppComponentRepository.GetApplicationComponentListbyappId(applicationId);
            return Ok(result);
        }

        // GET: api/Application/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _objAppComponentRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Application
        [HttpPost]
        public IHttpActionResult Post(ApplicationComponent value)
        {
            var result = _objAppComponentRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Application/5
        [HttpPut]
        public IHttpActionResult Put(int id, ApplicationComponent value)
        {
            var result = _objAppComponentRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<ApplicationComponent> value)
        {
            var result = _objAppComponentRepository.Find(id);
            value.Patch(result);
            return Ok(_objAppComponentRepository.Save(result));
        }

        // DELETE: api/Application/5
        public IHttpActionResult Delete(int id)
        {
            return Ok(_objAppComponentRepository.Delete(id));
        }

        [HttpGet]
        public IHttpActionResult InputLocations(int appId, int compId)
        {
            return Ok(_objInputLocationRepository.GetInputLocations(appId, compId));
        }

        [HttpPost]
        public IHttpActionResult AddInputLocation(ComponentInputLocation entity)
        {
            return Ok(_objInputLocationRepository.Save(entity));
        }

        [HttpDelete]
        public IHttpActionResult DeleteInputLocation(int id)
        {
            return Ok(_objInputLocationRepository.Delete(id));
        }

        [HttpGet]
        public IHttpActionResult OutputLocations(int appId, int compId)
        {
            return Ok(_objOutputLocationRepository.GetOutputLocations(appId, compId));
        }

        [HttpPost]
        public IHttpActionResult AddOutputLocation(ComponentOutputLocation entity)
        {
            return Ok(_objOutputLocationRepository.Save(entity));
        }

        [HttpDelete]
        public IHttpActionResult DeleteOutputLocation(int id)
        {
            return Ok(_objOutputLocationRepository.Delete(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _objAppComponentRepository.Dispose();
                _objInputLocationRepository.Dispose();
                _objOutputLocationRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
