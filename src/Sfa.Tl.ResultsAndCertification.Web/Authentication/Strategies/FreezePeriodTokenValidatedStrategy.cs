using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication.Strategies
{
    public class FreezePeriodTokenValidatedStrategy : ITokenValidatedStrategy
    {
        private readonly IDfeSignInApiClient _dfeSignInApiClient;
        private readonly IResultsAndCertificationInternalApiClient _resultsAndCertificationInternalApiClient;
        private readonly ISystemProvider _systemProvider;

        private readonly int _timeout;

        public FreezePeriodTokenValidatedStrategy(
                IDfeSignInApiClient dfeSignInApiClient,
                IResultsAndCertificationInternalApiClient resultsAndCertificationInternalApiClient,
                ISystemProvider systemProvider,
                ResultsAndCertificationConfiguration config)
        {
            _dfeSignInApiClient = dfeSignInApiClient;
            _resultsAndCertificationInternalApiClient = resultsAndCertificationInternalApiClient;
            _systemProvider = systemProvider;
            _timeout = config.DfeSignInSettings.Timeout;
        }

        public async Task GetOnTokenValidatedTask(TokenValidatedContext context)
        {
            IEnumerable<Claim> claims = await GetClaimsAsync(context);
            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "DfE-SignIn"));

            // so that we don't issue a session cookie but one with a fixed expiration
            context.Properties.IsPersistent = true;

            var cookieAndSessionTimeout = _timeout;
            var overallSessionTimeout = TimeSpan.FromMinutes(cookieAndSessionTimeout);
            context.Properties.ExpiresUtc = _systemProvider.UtcNow.Add(overallSessionTimeout);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(TokenValidatedContext context)
        {
            ClaimsPrincipal claimsPrincipal = context.Principal;

            var organisationId = GetOrganisationId(claimsPrincipal);
            if (string.IsNullOrEmpty(organisationId))
            {
                return new[] { CreateHasAccessToServiceClaim(false) };
            }

            var userId = GetUserId(claimsPrincipal);
            DfeUserInfo dfeUserInfo = await _dfeSignInApiClient.GetDfeSignInUserInfo(organisationId, userId);

            LoginUserType loginUseType = await GetLoginUserTypeAsync(dfeUserInfo);
            if (loginUseType == LoginUserType.NotSpecified)
            {
                return Enumerable.Empty<Claim>();
            }

            if (loginUseType != LoginUserType.Admin)
            {
                return new[]
                {
                    CreateHasAccessToServiceClaim(false),
                    CreateBooleanClaim(CustomClaimTypes.InFreezePeriod, true)
                };
            }

            IEnumerable<Claim> createdClaims = CreateClaims(claimsPrincipal, dfeUserInfo, loginUseType);
            IEnumerable<Claim> dfeUserInfoClaims = GetClaims(dfeUserInfo);

            return createdClaims.Concat(dfeUserInfoClaims);
        }

        private async Task<LoginUserType> GetLoginUserTypeAsync(DfeUserInfo dfeUserInfo)
        {
            if (IsAdminUser(dfeUserInfo))
            {
                return LoginUserType.Admin;
            }

            if (IsAwardingOrganisation(dfeUserInfo))
            {
                return LoginUserType.AwardingOrganisation;
            }

            LoggedInUserTypeInfo loggedInUserTypeInfo = await _resultsAndCertificationInternalApiClient.GetLoggedInUserTypeInfoAsync(dfeUserInfo.Ukprn.Value);
            return loggedInUserTypeInfo == null ? LoginUserType.NotSpecified : loggedInUserTypeInfo.UserType;
        }

        private static bool IsAdminUser(DfeUserInfo dfeUserInfo)
            => dfeUserInfo?.Ukprn.HasValue == false;

        private static bool IsAwardingOrganisation(DfeUserInfo dfeUserInfo)
            => dfeUserInfo?.Ukprn.Value == 1;

        private static IEnumerable<Claim> GetClaims(DfeUserInfo dfeUserInfo)
        {
            if (dfeUserInfo?.Roles == null || !dfeUserInfo.Roles.Any())
            {
                return Enumerable.Empty<Claim>();
            }

            return dfeUserInfo.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToList();
        }

        private static IEnumerable<Claim> CreateClaims(ClaimsPrincipal claimsPrincipal, DfeUserInfo dfeUserInfo, LoginUserType loginUserType)
            => new[]
            {
                // Claims principal
                new Claim(CustomClaimTypes.UserId, GetUserId(claimsPrincipal)),
                new Claim(CustomClaimTypes.OrganisationId, GetOrganisationId(claimsPrincipal)),
                new Claim(ClaimTypes.GivenName, claimsPrincipal.FindFirst("given_name").Value),
                new Claim(ClaimTypes.Surname, claimsPrincipal.FindFirst("family_name").Value),
                new Claim(ClaimTypes.Email, claimsPrincipal.FindFirst("email").Value),

                // DfE user info
                new Claim(CustomClaimTypes.Ukprn, dfeUserInfo.Ukprn.HasValue ? dfeUserInfo.Ukprn.Value.ToString() : string.Empty),
                new Claim(CustomClaimTypes.HasAccessToService, dfeUserInfo.HasAccessToService.ToString()),

                // Login user type
                new Claim(CustomClaimTypes.LoginUserType, ((int)loginUserType).ToString())
            };

        private static string GetOrganisationId(ClaimsPrincipal claimsPrincipal)
        {
            var organisationClaim = claimsPrincipal.FindFirst("Organisation");
            var organisation = JObject.Parse(organisationClaim.Value);

            return organisation.HasValues
                ? organisation.SelectToken("id").ToString()
                : string.Empty;
        }

        private static string GetUserId(ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirst("sub").Value;

        private static Claim CreateHasAccessToServiceClaim(bool hasAccess)
            => CreateBooleanClaim(CustomClaimTypes.HasAccessToService, hasAccess);

        private static Claim CreateBooleanClaim(string name, bool value)
            => new(name, value ? "true" : "false");
    }
}
