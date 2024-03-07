using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication.Strategies
{
    public delegate ITokenValidatedStrategy TokenValidatedStrategyResolver(DateTime freezePeriodFrom, DateTime freezePeriodTo);
}