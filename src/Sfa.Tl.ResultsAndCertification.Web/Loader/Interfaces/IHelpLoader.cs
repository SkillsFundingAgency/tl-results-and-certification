using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IHelpLoader
    {
        LoginUserType GetLoginUserType(ClaimsPrincipal claimsPrincipal);
    }
}