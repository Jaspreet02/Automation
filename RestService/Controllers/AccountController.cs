using Microsoft.AspNet.Identity;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MobileService.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController()
        {

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Account/ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await this.UserManager.ConfirmEmailAsync(userId, code);
                if (result.Succeeded) {
                    string url = Request.RequestUri.GetLeftPart(UriPartial.Authority).Replace("8001","8002/");
                    Uri uri = new Uri(url);
                    return Redirect(uri);
                }
                else { return BadRequest(); }
            }
            else { return NotFound(); }
        }

        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(ModelState);
            }

            return Ok();
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
