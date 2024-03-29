﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class RolesExtensions
    {
        // Awarding Organisation Policy Names
        public const string RequireTLevelsReviewerAccess = "RequireTLevelsReviewerAccess";
        public const string RequireProviderEditorAccess = "RequireProviderEditorAccess";
        public const string RequireRegistrationsEditorAccess = "RequireRegistrationsEditorAccess";
        public const string RequireResultsEditorAccess = "RequireResultsEditorAccess";
        public const string RequireReviewsAndAppealsEditorAccess = "RequireReviewsAndAppealsEditorAccess";

        // Awarding Organisation Roles
        public const string SiteAdministrator = "Site Administrator";
        public const string TlevelsReviewer = "T Levels Reviewer";
        public const string ProvidersEditor = "Providers Editor";
        public const string RegistrationsEditor = "Registrations Editor";
        public const string ResultsEditor = "Results Editor";

        // Training Provider Policy Names
        public const string RequireLearnerRecordsEditorAccess = "RequireLearnerRecordsEditorAccess";

        // TrainingProvider Roles
        public const string ProviderAdministrator = "Provider Administrator";
        public const string LearnerRecordsEditor = "Learner Records Editor";

        // Admin Dashboard Policy Names
        public const string RequireAdminDashboardAccess = "RequireAdminDashboardAccess";

        // Mixed Policy Names
        public const string RequireProviderEditorOrAdminDashboardAccess = "RequireProviderEditorOrAdminDashboardAccess";

        //Admin Dashboard Access
        public const string AdminDashboardAccess = "Admin Dashboard Access";

        public static bool HasAccessToService(this ClaimsPrincipal user)
        {
            var hasAccess = user.Claims.SingleOrDefault(c => c.Type == CustomClaimTypes.HasAccessToService)?.Value;

            if (bool.TryParse(hasAccess, out var result))
            {
                return result;
            }
            return false;
        }

        public static bool IsInFreezePeriod(this ClaimsPrincipal user)
        {
            var hasAccess = user.Claims.SingleOrDefault(c => c.Type == CustomClaimTypes.InFreezePeriod)?.Value;

            if (bool.TryParse(hasAccess, out var result))
            {
                return result;
            }
            return false;
        }

        public static bool HasSiteAdministratorRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(SiteAdministrator);
        }

        public static bool HasTlevelsReviewerRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(TlevelsReviewer);
        }

        public static bool HasProvidersEditorRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(ProvidersEditor);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNames = user.Claims.Where(c => c.Type == ClaimTypes.GivenName || c.Type == ClaimTypes.Surname).Select(c => c.Value);
            return string.Join(" ", userNames);
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;
        }

        public static long GetUkPrn(this ClaimsPrincipal user)
        {
            var ukprn = user?.Claims?.SingleOrDefault(c => c.Type == CustomClaimTypes.Ukprn)?.Value;
            return !string.IsNullOrWhiteSpace(ukprn) ? long.Parse(ukprn) : 0;
        }

        public static LoginUserType? GetLoggedInUserType(this ClaimsPrincipal user)
        {
            var userType = user.Claims.SingleOrDefault(c => c.Type == CustomClaimTypes.LoginUserType)?.Value;
            return EnumExtensions.IsValidValue<LoginUserType>(userType) ? EnumExtensions.GetEnum<LoginUserType>(userType) : (LoginUserType?)null;
        }

        public static bool HasLoginUserTypeClaimAndRole(this ClaimsPrincipal user, LoginUserType userType, string role, params string[] extraRoles)
        {
            return user.HasLoginUserTypeClaim(userType) && user.HasRole(role, extraRoles);
        }

        private static bool HasRole(this ClaimsPrincipal user, string role, params string[] extraRoles)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            var roles = new List<string> { role };

            if (!extraRoles.IsNullOrEmpty())
            {
                roles.AddRange(extraRoles);
            }

            return roles.All(r => user.IsInRole(role));
        }

        private static bool HasLoginUserTypeClaim(this ClaimsPrincipal user, LoginUserType userType)
        {
            LoginUserType? loginUserType = user.GetLoggedInUserType();
            return loginUserType.HasValue ? loginUserType.Value == userType : false;
        }
    }
}