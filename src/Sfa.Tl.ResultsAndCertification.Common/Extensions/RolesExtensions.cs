using System.Linq;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class RolesExtensions
    {
        public static bool HasAccessToService(this ClaimsPrincipal user)
        {
            var hasAccess = user.Claims.SingleOrDefault(c => c.Type == CustomClaimTypes.HasAccessToService)?.Value;
            
            if (bool.TryParse(hasAccess, out var result))
            {
                return result;
            }
            return false;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNames = user.Claims.Where(c => c.Type == ClaimTypes.GivenName || c.Type == ClaimTypes.Surname).Select(c => c.Value);
            return string.Join(" ", userNames);
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Upn)?.Value;
        }
    }
}
