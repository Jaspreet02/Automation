using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace MobileService.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController()
        {

        }

        [AllowAnonymous]
        [Route("api/account/confirmemail", Name = "ConfirmEmail")]
        [HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmEmailBindingModel model)
        {
            var user = await this.UserManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                var result = await this.UserManager.ConfirmEmailAsync(model.UserId, model.Code);
                if (result.Succeeded) { return Ok(); }
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
