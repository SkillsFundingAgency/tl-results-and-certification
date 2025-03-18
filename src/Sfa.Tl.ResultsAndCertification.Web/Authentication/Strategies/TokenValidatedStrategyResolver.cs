using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication.Strategies
{
    public delegate ITokenValidatedStrategy TokenValidatedStrategyResolver(ServiceFreezePeriods serviceFreezePeriods);
}