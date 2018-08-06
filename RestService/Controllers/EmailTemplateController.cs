using DbHander;
using MobileService.Common;
using System;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    [Authorize]
    public class EmailTemplateController : BaseController
    {
        IEmailTemplateRepository _objEmailTemplateRepository;

        public EmailTemplateController()
        {
            _objEmailTemplateRepository = new EmailTemplateRepository();
        }

        // GET: api/Client
        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = _objEmailTemplateRepository.FindAll();
            return Ok(result);
        }

        // GET: api/Client/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _objEmailTemplateRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Client
        [HttpPost]
        public IHttpActionResult Post(EmailTemplate value)
        {
            var result = _objEmailTemplateRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Client/5
        [HttpPut]
        public IHttpActionResult Put(int id, EmailTemplate value)
        {
            var result = _objEmailTemplateRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<EmailTemplate> value)
        {
            var result = _objEmailTemplateRepository.Find(id);
            value.Patch(result);
            return Ok(_objEmailTemplateRepository.Save(result));
        }

        // DELETE: api/Client/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return Ok(_objEmailTemplateRepository.Delete(id));
        }

        [HttpGet]
        public IHttpActionResult EmailToken()
        {
            return Ok(Enum.GetNames(typeof(EmailKeyword)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _objEmailTemplateRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
