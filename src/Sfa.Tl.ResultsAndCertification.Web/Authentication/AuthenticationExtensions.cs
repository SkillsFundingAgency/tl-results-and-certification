using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Authentication.Local;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddWebAuthentication(this IServiceCollection services, ResultsAndCertificationConfiguration config, IWebHostEnvironment env)
        {
            var cookieSecurePolicy = env.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;

            if (config.EnableLocalAuthentication)
            {
                services.AddAntiforgery(options =>
                {
                    options.Cookie.SecurePolicy = cookieSecurePolicy;
                });

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddLocalAuthentication(o =>
                {
                    o.HasAccessToService = true;
                    o.Ukprn = "10011881";
                    o.Identity = o.ClaimsIdentity;
                });
                return services;
            }
            else
            {
                var cookieAndSessionTimeout = config.DfeSignInSettings.Timeout;
                var overallSessionTimeout = TimeSpan.FromMinutes(cookieAndSessionTimeout);

                services.AddAntiforgery(options =>
                {
                    options.Cookie.SecurePolicy = cookieSecurePolicy;
                });

                services.AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = "tl-rc-auth-cookie";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = overallSessionTimeout;
                    options.LogoutPath = config.DfeSignInSettings.LogoutPath;
                    options.AccessDeniedPath = "/access-denied";
                })
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.MetadataAddress = config.DfeSignInSettings.MetadataAddress;
                    options.RequireHttpsMetadata = false;
                    options.ClientId = config.DfeSignInSettings.ClientId;
                    options.ClientSecret = config.DfeSignInSettings.ClientSecret;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.Scope.Add("profile");
                    options.Scope.Add("organisation");

                    // When we expire the session, ensure user is prompted to sign in again at DfE Sign In
                    options.MaxAge = overallSessionTimeout;

                    options.SaveTokens = true;
                    options.CallbackPath = new PathString(config.DfeSignInSettings.CallbackPath);
                    options.SignedOutCallbackPath = new PathString(config.DfeSignInSettings.SignedOutCallbackPath);
                    options.SignedOutRedirectUri = "/signout-complete";
                    options.SecurityTokenValidator = new JwtSecurityTokenHandler
                    {
                        InboundClaimTypeMap = new Dictionary<string, string>(),
                        TokenLifetimeInMinutes = cookieAndSessionTimeout,
                        SetDefaultTimesOnTokenCreation = true,
                    };
                    options.ProtocolValidator = new OpenIdConnectProtocolValidator
                    {
                        RequireSub = true,
                        RequireStateValidation = false,
                        NonceLifetime = overallSessionTimeout
                    };

                    options.DisableTelemetry = true;

                    options.Events = new OpenIdConnectEvents
                    {
                        // Sometimes, problems in the OIDC provider (such as session timeouts)
                        // Redirect the user to the /auth/cb endpoint. ASP.NET Core middleware interprets this by default
                        // as a successful authentication and throws in surprise when it doesn't find an authorization code.
                        // This override ensures that these cases redirect to the root.
                        OnMessageReceived = context =>
                            {
                                var isSpuriousAuthCbRequest =
                                    context.Request.Path == options.CallbackPath &&
                                    context.Request.Method == "GET" &&
                                    !context.Request.Query.ContainsKey("code");

                                if (isSpuriousAuthCbRequest)
                                {
                                    context.HandleResponse();
                                    context.Response.Redirect("/");
                                }
                                return Task.CompletedTask;
                            },

                        // Sometimes the auth flow fails. The most commonly observed causes for this are
                        // Cookie correlation failures, caused by obscure load balancing stuff.
                        // In these cases, rather than send user to a 500 page, prompt them to re-authenticate.
                        // This is derived from the recommended approach: https://github.com/aspnet/Security/issues/1165
                        OnRemoteFailure = ctx =>
                        {
                            ctx.HandleResponse();
                            return Task.FromException(ctx.Failure);
                        },

                        // that event is called after the OIDC middleware received the authorisation code,
                        // redeemed it for an access token and a refresh token,
                        // and validated the identity token
                        OnTokenValidated = async ctx =>
                        {
                            var organisation = JObject.Parse(ctx.Principal.FindFirst("Organisation").Value);
                            var organisationId = organisation["id"].ToString();
                            var userId = ctx.Principal.FindFirst("sub").Value;
                            var ukprn = organisation["ukprn"].ToObject<int?>();
                            var dfeSignInApiClient = ctx.HttpContext.RequestServices.GetService<IDfeSignInApiClient>();
                            var userInfo = await dfeSignInApiClient.GetUserInfo(organisationId, userId);

                            var claims = new List<Claim>()
                            {
                                new Claim(CustomClaimTypes.HasAccessToService, userInfo.HasAccessToService.ToString()),
                                new Claim(CustomClaimTypes.UserId, userId),
                                new Claim(ClaimTypes.GivenName, ctx.Principal.FindFirst("given_name").Value),
                                new Claim(ClaimTypes.Surname, ctx.Principal.FindFirst("family_name").Value),
                                new Claim(ClaimTypes.Email, ctx.Principal.FindFirst("email").Value),
                                new Claim(CustomClaimTypes.Ukprn, ukprn.HasValue ? ukprn.Value.ToString() : string.Empty),
                                new Claim(CustomClaimTypes.OrganisationId, organisationId)
                            };

                            if (userInfo.Roles != null && userInfo.Roles.Any())
                            {
                                claims.AddRange(userInfo.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
                            }

                            ctx.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "DfE-SignIn"));

                            // so that we don't issue a session cookie but one with a fixed expiration
                            ctx.Properties.IsPersistent = true;
                            ctx.Properties.ExpiresUtc = DateTime.UtcNow.Add(overallSessionTimeout);
                        }
                    };
                });
                return services;
            }
        }
    }
}
