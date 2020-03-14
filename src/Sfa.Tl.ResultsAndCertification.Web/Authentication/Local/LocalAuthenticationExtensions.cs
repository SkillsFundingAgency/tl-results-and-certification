using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication.Local
{
    public static class LocalAuthenticationExtensions
    {
        public static AuthenticationBuilder AddLocalAuthentication(this AuthenticationBuilder builder, Action<LocalAuthenticationSchemeOptions> configureOptions)
        {
            return builder.AddScheme<LocalAuthenticationSchemeOptions, LocalAuthenticationHandler>(CookieAuthenticationDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }
    }
}
