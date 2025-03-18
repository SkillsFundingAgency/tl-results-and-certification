using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication.Strategies
{
    public class TokenValidatedStrategy : ITokenValidatedStrategy
    {
        private readonly IDfeSignInApiClient _dfeSignInApiClient;
        private readonly IResultsAndCertificationInternalApiClient _resultsAndCertificationInternalApiClient;
        private readonly int _timeout;

        public TokenValidatedStrategy(
                IDfeSignInApiClient dfeSignInApiClient,
                IResultsAndCertificationInternalApiClient resultsAndCertificationInternalApiClient,
                ResultsAndCertificationConfiguration config)
        {
            _dfeSignInApiClient = dfeSignInApiClient;
            _resultsAndCertificationInternalApiClient = resultsAndCertificationInternalApiClient;
            _timeout = config.DfeSignInSettings.Timeout;
        }

        public async Task GetOnTokenValidatedTask(TokenValidatedContext context)
        {
            var claims = new List<Claim>();
            var organisation = JObject.Parse(context.Principal.FindFirst("Organisation").Value);

            if (organisation.HasValues)
            {
                var organisationId = organisation.SelectToken("id").ToString();
                var userId = context.Principal.FindFirst("sub").Value;
                var userInfo = await _dfeSignInApiClient.GetDfeSignInUserInfo(organisationId, userId);

                if (userInfo.HasAccessToService)
                {
                    var loggedInUserTypeResponse = !userInfo.Ukprn.HasValue ? new LoggedInUserTypeInfo { UserType = LoginUserType.Admin } : userInfo.Ukprn.Value == 1
                                                    ? new LoggedInUserTypeInfo { Ukprn = userInfo.Ukprn.Value, UserType = LoginUserType.AwardingOrganisation }
                                                    : await _resultsAndCertificationInternalApiClient.GetLoggedInUserTypeInfoAsync(userInfo.Ukprn.Value);

                    if (loggedInUserTypeResponse != null && loggedInUserTypeResponse.UserType != LoginUserType.NotSpecified)
                    {
                        claims.AddRange(new List<Claim>
                                        {
                                            new Claim(CustomClaimTypes.UserId, userId),
                                            new Claim(CustomClaimTypes.OrganisationId, organisationId),
                                            new Claim(CustomClaimTypes.Ukprn, userInfo.Ukprn.HasValue ? userInfo.Ukprn.Value.ToString() : string.Empty),
                                            new Claim(ClaimTypes.GivenName, context.Principal.FindFirst("given_name").Value),
                                            new Claim(ClaimTypes.Surname, context.Principal.FindFirst("family_name").Value),
                                            new Claim(ClaimTypes.Email, context.Principal.FindFirst("email").Value),
                                            new Claim(CustomClaimTypes.HasAccessToService, userInfo.HasAccessToService.ToString()),
                                            new Claim(CustomClaimTypes.LoginUserType, ((int)loggedInUserTypeResponse.UserType).ToString())
                                        });

                        if (userInfo.Roles != null && userInfo.Roles.Any())
                        {
                            claims.AddRange(userInfo.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
                        }
                    }

                }
                else
                { claims.Add(new Claim(CustomClaimTypes.HasAccessToService, "true")); }
            }
            else
            {
                claims.Add(new Claim(CustomClaimTypes.HasAccessToService, "false"));
            }

            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "DfE-SignIn"));

            // so that we don't issue a session cookie but one with a fixed expiration
            context.Properties.IsPersistent = true;

            var overallSessionTimeout = TimeSpan.FromMinutes(_timeout);
            context.Properties.ExpiresUtc = DateTime.UtcNow.Add(overallSessionTimeout);
        }
    }
}