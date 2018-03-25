using DbHander;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    public class ProofController : ApiController
    {
        IProofRepository _objProofRepository;

        public ProofController()
        {
            _objProofRepository = new ProofRepository();
        }

        // GET: api/Client
        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = _objProofRepository.FindAllActive();
            return Ok(result);
        }

        // GET: api/Client/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _objProofRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Client
        [HttpPost]
        public IHttpActionResult Post(Proof value)
        {
            var result = _objProofRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Client/5
        [HttpPut]
        public IHttpActionResult Put(int id, Proof value)
        {
            var result = _objProofRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<Proof> value)
        {
            var result = _objProofRepository.Find(id);
            value.Patch(result);
            return Ok(_objProofRepository.Save(result));
        }

        // DELETE: api/Client/5
        public IHttpActionResult Delete(int id)
        {
            return Ok(_objProofRepository.Delete(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _objProofRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
