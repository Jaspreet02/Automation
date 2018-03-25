using Microsoft.AspNet.Identity;

namespace MobileService
{
    public interface ISendEmailService
    {
        bool SendEmail(IdentityMessage message);
    }
}
