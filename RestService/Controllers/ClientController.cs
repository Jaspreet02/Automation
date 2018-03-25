﻿using DbHander;
using Microsoft.AspNet.Identity;
using MobileService.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    public class ClientController : BaseController
    {
        IClientRepository _objclientRepository;

        public ClientController()
        {
            _objclientRepository = new ClientRepository();
        }

        // GET: api/Client
        [HttpGet]
        public async Task<IHttpActionResult> Get(int pageNumber = 1, int pageSize = 10, bool fetchAll = false)
        {
            // var userList = GenericPrincipalExtensions.Users(User);
            var result = _objclientRepository.FindAll().OrderByDescending(x => x.CreatedAt);  //_objclientRepository.FindAll().Where(x => userList.Contains(x.UserId)).OrderByDescending(x => x.CreatedAt);
            return Ok(await CreatePageResult<Client>(result, pageNumber, pageSize, fetchAll));
        }

        // GET: api/Client/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _objclientRepository.Find(id);
            return Ok(result);
        }

        // POST: api/Client
        [HttpPost]
        public IHttpActionResult Post(Client value)
        {
            value.UserId =User.Identity.GetUserId();
            var result = _objclientRepository.Save(value);
            return Ok(result);
        }

        // PUT: api/Client/5
        [HttpPut]
        public IHttpActionResult Put(int id, Client value)
        {
            value.UserId = User.Identity.GetUserId();
            var result = _objclientRepository.Save(value);
            return Ok(result);
        }

        public IHttpActionResult Patch(int id, Delta<Client> value)
        {
            var result = _objclientRepository.Find(id);
            value.Patch(result);
            return Ok(_objclientRepository.Save(result));
        }

        // DELETE: api/Client/5
        public IHttpActionResult Delete(int id)
        {
            return Ok(_objclientRepository.Delete(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _objclientRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
