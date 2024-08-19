using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Authentication.Local;
using Sfa.Tl.ResultsAndCertification.Web.Authentication.Strategies;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddWebAuthentication(this IServiceCollection services, ResultsAndCertificationConfiguration config, IWebHostEnvironment env)
        {
            var cookieSecurePolicy = env.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;

            if (config.BypassDfeSignIn)
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
                    o.Ukprn = "10009696";
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
                    options.AccessDeniedPath = "/access-denied-wrong-role";
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
                    options.NonceCookie.SameSite = SameSiteMode.None;
                    options.CorrelationCookie.SameSite = SameSiteMode.None;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.Scope.Add("profile");
                    options.Scope.Add("organisationid");

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
                        OnMessageReceived = async context =>
                        {
                            bool isSpuriousAuthCbRequest =
                                context.Request.Path == options.CallbackPath &&
                                context.Request.Method == "GET" &&
                                !context.Request.Query.ContainsKey("code");

                            if (isSpuriousAuthCbRequest)
                            {
                                HttpRequest request = context.Request;

                                var logger = context.GetLogger();
                                logger.LogError("[On Message Received]: IsSpurious: {IsSpuriousAuthCbRequest} " +
                                    "| Request method: {Method} " +
                                    "| Request path: {Path} " +
                                    "| Request query: {Query} ", isSpuriousAuthCbRequest, request.Method, request.Path, request.Query);

                                context.HandleResponse();
                                context.Response.Redirect("/");
                            }
                        },

                        // Sometimes the auth flow fails. The most commonly observed causes for this are
                        // Cookie correlation failures, caused by obscure load balancing stuff.
                        // In these cases, rather than send user to a 500 page, prompt them to re-authenticate.
                        // This is derived from the recommended approach: https://github.com/aspnet/Security/issues/1165
                        OnRemoteFailure = context =>
                        {
                            var logger = context.GetLogger();

                            string message = context.Failure?.Message;
                            logger.LogError(context.Failure, "[On Remote Failure]: {Message}", message);

                            context.HandleResponse();
                            context.Response.Redirect("/");

                            return Task.CompletedTask;
                        },

                        // that event is called after the OIDC middleware received the authorisation code,
                        // redeemed it for an access token and a refresh token,
                        // and validated the identity token
                        OnTokenValidated = context =>
                        {
                            var logger = context.GetLogger();

                            string message = context.Principal.ToString();
                            logger.LogError(new FormatException(message), "[On Token Validated]: {Message}", message);

                            var resolver = context.GetService<TokenValidatedStrategyResolver>();
                            ITokenValidatedStrategy strategy = resolver(config.FreezePeriodStartDate, config.FreezePeriodEndDate);

                            return strategy.GetOnTokenValidatedTask(context);
                        }
                    };
                });
                return services;
            }
        }

        private static TService GetService<TService>(this TokenValidatedContext context) where TService : class
             => context?.HttpContext?.RequestServices?.GetService<TService>();

        private static ILogger GetLogger(this RemoteFailureContext context)
        {
            var loggerProvider = context?.HttpContext?.RequestServices?.GetService<ILoggerProvider>();
            return loggerProvider.CreateLogger("Authentication");
        }

        private static ILogger GetLogger(this TokenValidatedContext context)
        {
            var loggerProvider = context?.HttpContext?.RequestServices?.GetService<ILoggerProvider>();
            return loggerProvider.CreateLogger("Authentication");
        }

        private static ILogger GetLogger(this MessageReceivedContext context)
        {
            var loggerProvider = context?.HttpContext?.RequestServices?.GetService<ILoggerProvider>();
            return loggerProvider.CreateLogger("Authentication");
        }
    }
}