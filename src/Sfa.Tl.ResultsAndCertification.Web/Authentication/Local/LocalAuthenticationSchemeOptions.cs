using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication.Local
{
    public class LocalAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; set; }
        public string Ukprn { get; set; }
        public bool HasAccessToService { get; set; }

        public ClaimsIdentity ClaimsIdentity => new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, "test.user@test.com"),
            new Claim(ClaimTypes.GivenName, "Test"),
            new Claim(ClaimTypes.Surname, "User"),
            new Claim(ClaimTypes.Role, RolesExtensions.SiteAdministrator),
            new Claim(CustomClaimTypes.HasAccessToService, HasAccessToService.ToString()),            
            new Claim(CustomClaimTypes.Ukprn, string.IsNullOrWhiteSpace(Ukprn) ? Ukprn : "10009696"),
            new Claim(CustomClaimTypes.LoginUserType, ((int)LoginUserType.AwardingOrganisation).ToString())
        }, "local");
    }
}
