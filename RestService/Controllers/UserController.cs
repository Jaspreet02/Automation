using DbHander;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MobileService.App_Start;
using MobileService.Common;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;

namespace MobileService.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        IUserRepository _objUserRepository;

        public UserController()
        {
            _objUserRepository = new UserRepository();
        }

        // GET: api/Client
        [HttpGet]
        public async Task<IHttpActionResult> Get(int pageNumber = 0, int pageSize = 10,string sortField = "CreatedAt", string sortOrder = "desc", bool fetchAll = false)
        {
            var userList = GenericPrincipalExtensions.Users(User);
            var result = _objUserRepository.FindAll().Where(x => userList.Contains(x.Id)).OrderBy(sortField + " " + sortOrder);
            return Ok(await CreatePageResult<ApplicationUser>(result, pageNumber, pageSize, fetchAll));
        }

        // GET: api/Client/51
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            var result = UserManager.FindByIdAsync(id);
            return Ok(await result);
        }

        // POST: api/Client
        [HttpPost]
        public async Task<IHttpActionResult> Post(ApplicationUser value, string roleName)
        {
            value.ParentId = User.Identity.GetUserId();
            value.UserName = value.Email;
            const string password = "Gurdit@s1ngh007";
            value.PasswordHash = password;
            value.CreatedAt = DateTimeOffset.Now;
            IdentityResult result = UserManager.Create(value, value.PasswordHash);
            if (result.Succeeded)
            {
                result = UserManager.AddToRole(value.Id, roleName);
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(value.Id);
                var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new{ userId = value.Id, code = code }));
                await UserManager.SendEmailAsync(value.Id,
               "Confirm your account",
               "Please confirm your account by clicking this link: <a href=\""
                                               + callbackUrl + "\">link</a>");
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Client/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(string id, ApplicationUser value)
        {
            // Get the existing student from the db
            var user =await UserManager.FindByIdAsync(id);

            // Update it with the values from the view model
            user.LastName = value.LastName;
            user.FirstName = value.FirstName;
            user.UserName = value.UserName;
            user.Email = value.Email;
            user.PhoneNumber = value.PhoneNumber;
            user.Gender = value.Gender;
            user.Status = value.Status;
            // Apply the changes if any to the db
             await UserManager.UpdateAsync(user);
            return Ok(user.Id);
        }

        [HttpPatch]
        public IHttpActionResult Patch(int id, Delta<ApplicationUser> value)
        {
            var result = UserManager.FindById(id.ToString());
            value.Patch(result);
            return Ok(UserManager.CreateAsync(result));
        }

        // DELETE: api/Client/5
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            return Ok(UserManager.Delete(UserManager.FindById(id)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
