namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class CustomClaimTypes
    {
        /// <summary>
        /// The access token
        /// </summary>
        public const string AccessToken = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/accesstoken";

        /// <summary>
        /// The refresh token
        /// </summary>
        public const string RefreshToken = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/refreshtoken";

        /// <summary>
        /// The has access to service
        /// </summary>
        public const string HasAccessToService = "http://schemas.microsoft.com/ws/2008/06/identity/claims/hasaccesstoservice";

        /// <summary>
        /// The ukprn
        /// </summary>
        public const string Ukprn = "http://schemas.microsoft.com/ws/2008/06/identity/claims/ukprn";

        /// <summary>
        /// The organisation identifier
        /// </summary>
        public const string OrganisationId = "http://schemas.microsoft.com/ws/2008/06/identity/claims/organisationid";

        /// <summary>
        /// The user identifier
        /// </summary>
        public const string UserId = "http://schemas.microsoft.com/ws/2008/06/identity/claims/userid";
    }
}
