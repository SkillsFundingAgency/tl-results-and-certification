using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddWebAuthentication(this IServiceCollection services, ResultsAndCertificationConfiguration config, IWebHostEnvironment env)
        {
            double cookieAndSessionTimeout = 2;
            var overallSessionTimeout = TimeSpan.FromMinutes(cookieAndSessionTimeout);
            var cookieSecurePolicy = env.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
            
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
                options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieAndSessionTimeout);
                options.LogoutPath = config.DfeSignInSettings.LogoutPath;
                options.AccessDeniedPath = "/error/accessdenied";
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
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
                options.Scope.Add("offline_access");

                // Prompt=consent is required to be issued with a refresh token
                options.Prompt = "consent";

                // When we expire the session, ensure user is prompted to sign in again at DfE Sign In
                options.MaxAge = overallSessionTimeout;

                options.SaveTokens = true;
                options.CallbackPath = new PathString(config.DfeSignInSettings.CallbackPath);
                options.SignedOutCallbackPath = new PathString(config.DfeSignInSettings.SignedOutCallbackPath);
                options.SignedOutRedirectUri = "/account/signoutcomplete";
                options.SecurityTokenValidator = new JwtSecurityTokenHandler
                {
                    InboundClaimTypeMap = new Dictionary<string, string>(),
                    TokenLifetimeInMinutes = int.Parse(cookieAndSessionTimeout.ToString()),
                    SetDefaultTimesOnTokenCreation = true,
                };
                options.ProtocolValidator = new OpenIdConnectProtocolValidator
                {
                    RequireSub = true,
                    RequireStateValidation = false,
                    NonceLifetime = TimeSpan.FromMinutes(cookieAndSessionTimeout)
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
                            context.Response.StatusCode = 302;
                            context.Response.Headers["Location"] = "/";
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

                    OnRedirectToIdentityProvider = context =>
                    {
                        return Task.CompletedTask;
                    },

                    // that event is called after the OIDC middleware received the authorisation code,
                    // redeemed it for an access token and a refresh token,
                    // and validated the identity token
                    OnTokenValidated = async x =>
                    {
                        var cliendId = config.DfeSignInSettings.ClientId;
                        var issuer = config.DfeSignInSettings.Issuer;
                        var audience = config.DfeSignInSettings.Audience;
                        var apiSecret = config.DfeSignInSettings.ApiSecret;
                        var apiUri = config.DfeSignInSettings.ApiUri;

                        Throw.IfNull(issuer, nameof(issuer));
                        Throw.IfNull(audience, nameof(audience));
                        Throw.IfNull(apiSecret, nameof(apiSecret));
                        Throw.IfNull(apiUri, nameof(apiUri));

                        var token = new JwtBuilder()
                            .WithAlgorithm(new HMACSHA256Algorithm())
                            .Issuer(issuer)
                            .Audience(audience)
                            .WithSecret(apiSecret)
                            .Build();

                        //Gather user/org details
                        var identity = (ClaimsIdentity)x.Principal.Identity;
                        Organisation organisation;
                        try
                        {
                            organisation = JsonConvert.DeserializeObject<Organisation>(
                            identity.Claims.Where(c => c.Type == "organisation")
                            .Select(c => c.Value).FirstOrDefault());
                        }
                        catch
                        {
                            throw new SystemException("Unable to get organisation details from DFE. Please clear session cookies, or try using private browsing mode.");
                        }

                        var userClaims = new DfeClaims()
                        {
                            UserId = Guid.Parse(identity.Claims.Where(c => c.Type == "sub").Select(c => c.Value).SingleOrDefault()),
                            ServiceId = Guid.Parse(identity.Claims.Where(c => c.Type == "sid").Select(c => c.Value).SingleOrDefault()),
                        };

                        var client = new HttpClient();
                        client.SetBearerToken(token);
                        var response = await client.GetAsync($"{apiUri}/services/{cliendId}/organisations/{organisation.Id}/users/{userClaims.UserId}");

                        if (response.IsSuccessStatusCode)
                        {
                            var json = response.Content.ReadAsStringAsync().Result;
                            userClaims = JsonConvert.DeserializeObject<DfeClaims>(json);
                            userClaims.RoleName = userClaims.Roles.Select(r => r.Name).FirstOrDefault();
                            userClaims.UKPRN = organisation.UKPRN.HasValue ? organisation.UKPRN.Value.ToString() : string.Empty;
                            userClaims.UserName = identity.Claims.Where(c => c.Type == "email").Select(c => c.Value).SingleOrDefault();
                        }
                        else
                        {
                            throw new SystemException("Could not get Role Type for User");
                        }

                        // store both access and refresh token in the claims - hence in the cookie
                        identity.AddClaims(new[]
                        {
                                new Claim("access_token", x.TokenEndpointResponse.AccessToken),
                                new Claim("refresh_token", x.TokenEndpointResponse.RefreshToken),
                                new Claim("user_id", userClaims.UserId.ToString()),
                                new Claim(ClaimTypes.Role, userClaims.RoleName),
                        });

                        // so that we don't issue a session cookie but one with a fixed expiration
                        x.Properties.IsPersistent = true;

                        x.Properties.ExpiresUtc = DateTime.UtcNow.Add(overallSessionTimeout);
                    }
                };
            });

            return services;
        }
    }
}
