using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
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
        private readonly ResultsAndCertificationConfiguration _config;
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
            _config = config;
        }

        public async Task GetOnTokenValidatedTask(TokenValidatedContext context)
        {
            IEnumerable<Claim> claims = await GetClaimsAsync(context);
            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "DfE-SignIn"));

            // so that we don't issue a session cookie but one with a fixed expiration
            context.Properties.IsPersistent = true;

            var cookieAndSessionTimeout = _config.DfeSignInSettings.Timeout;
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

            LoginUserType loginUserType = await GetLoginUserTypeAsync(dfeUserInfo);
            if (loginUserType == LoginUserType.NotSpecified)
            {
                return Enumerable.Empty<Claim>();
            }

            if (loginUserType != LoginUserType.Admin && UserInFreezePeriod(loginUserType))
                return GetUserClaims(loginUserType);

            IEnumerable<Claim> createdClaims = CreateClaims(claimsPrincipal, dfeUserInfo, loginUserType);
            IEnumerable<Claim> dfeUserInfoClaims = GetClaims(dfeUserInfo);

            return createdClaims.Concat(dfeUserInfoClaims);
        }

        private bool UserInFreezePeriod(LoginUserType loginUserType) => loginUserType switch
        {
            LoginUserType.TrainingProvider => InFreezePeriod(
                _config.ServiceFreezePeriodsSettings.TrainingProvider.StartDate,
                _config.ServiceFreezePeriodsSettings.TrainingProvider.EndDate),

            LoginUserType.AwardingOrganisation => InFreezePeriod(
                _config.ServiceFreezePeriodsSettings.AwardingOrganisation.StartDate,
                _config.ServiceFreezePeriodsSettings.AwardingOrganisation.EndDate),
            _ => throw new Exception("Invalid user type")
        };

        private Claim[] GetUserClaims(LoginUserType loginUserType, bool freezePeriod = true) => loginUserType switch
        {
            LoginUserType.TrainingProvider => GetFreezePeriodClaims(loginUserType, freezePeriod),
            LoginUserType.AwardingOrganisation => GetFreezePeriodClaims(loginUserType, freezePeriod),
            _ => throw new NotImplementedException()
        };

        private Claim[] GetFreezePeriodClaims(LoginUserType loginUserType, bool isFreezePeriod) => new[]{
                CreateHasAccessToServiceClaim(!isFreezePeriod),
                CreateUserTypeClaim(((int)loginUserType).ToString()),
                CreateBooleanClaim(CustomClaimTypes.InFreezePeriod, isFreezePeriod)
            };
        private Claim[] GetProviderClaims(LoginUserType loginUserType)
        {
            var isFreezePeriod = InFreezePeriod(_config.ServiceFreezePeriodsSettings.TrainingProvider.StartDate,
                _config.ServiceFreezePeriodsSettings.TrainingProvider.EndDate);

            return new[]{
                CreateHasAccessToServiceClaim(!isFreezePeriod),
                CreateUserTypeClaim(((int)loginUserType).ToString()),
                CreateBooleanClaim(CustomClaimTypes.InFreezePeriod, isFreezePeriod),
            };
        }

        private Claim[] GetAwardingOrganisationClaims(LoginUserType loginUserType)
        {
            var isFreezePeriod = InFreezePeriod(_config.ServiceFreezePeriodsSettings.AwardingOrganisation.StartDate,
                        _config.ServiceFreezePeriodsSettings.AwardingOrganisation.EndDate);

            return new[]{
                CreateHasAccessToServiceClaim(!isFreezePeriod),
                CreateUserTypeClaim(((int)loginUserType).ToString()),
                CreateBooleanClaim(CustomClaimTypes.InFreezePeriod, isFreezePeriod)
            };
        }

        private bool InFreezePeriod(DateTime startDate, DateTime endDate)
        {
            var freezePeriods = new DateTimeRange
            {
                From = startDate,
                To = endDate
            };

            return freezePeriods.Contains(_systemProvider.UtcNow);
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
                new Claim(CustomClaimTypes.LoginUserType, ((int)loginUserType).ToString()),
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

        private static Claim CreateUserTypeClaim(string usertype)
            => new Claim(CustomClaimTypes.LoginUserType, usertype);

    }
}
