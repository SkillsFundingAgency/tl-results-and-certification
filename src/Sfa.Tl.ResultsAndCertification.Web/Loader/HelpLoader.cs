using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class HelpLoader : IHelpLoader
    {
        public LoginUserType GetLoginUserType(ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.GetLoggedInUserType().Value;
    }
}