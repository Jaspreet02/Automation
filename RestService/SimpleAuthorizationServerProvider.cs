using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DbHander;
using System.Linq;
using MobileService.Common;
using Microsoft.AspNet.Identity.Owin;
using MobileService.App_Start;
using Microsoft.AspNet.Identity;

namespace MobileService
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        IUserRepository userRepository = new UserRepository();
        IClientRepository clientRepository = new ClientRepository();
        IApplicationRepository applicationRepository = new ApplicationRepository();
       // IUserRoleRepository userRoleRepository = new UserRoleRepository();

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            var userRoles = userManager.GetRoles(user.Id);
            identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(";", userRoles)));
            var userList = userRepository.UserIds(user.Id).ToList();
            identity.AddClaim(new Claim("UserList", string.Join(";", userList)));
            var clientList = clientRepository.ClientIdsbyUser(userList).ToList();
            identity.AddClaim(new Claim("ClientList", string.Join(";", clientList)));
            identity.AddClaim(new Claim("ApplicationList", string.Join(";", applicationRepository.ApplicationIdsbyClients(clientList).Select(x => x.Key + ":" + string.Join(",", x.Value)).ToArray())));
            var ticket = new AuthenticationTicket(identity, CreateProperties(user.UserName, userRoles.ToList()));

            context.Validated(ticket);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        private static AuthenticationProperties CreateProperties(string userName, List<string> role)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
              {
                  { "userName", userName},
                  { "role" , role.Contains("superAdmin") ? "superAdmin" : role.Contains("admin") ? "admin" : "user"}
              };
            return new AuthenticationProperties(data);
        }
    }
}