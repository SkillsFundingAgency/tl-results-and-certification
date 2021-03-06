﻿namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class DfeSignInSettings
    {
        /// <summary>Gets or sets the metadata address.</summary>
        /// <value>The metadata address.</value>
        public string MetadataAddress { get; set; }

        /// <summary>Gets or sets the client identifier.</summary>
        /// <value>The client identifier.</value>
        public string ClientId { get; set; }

        /// <summary>Gets or sets the client secret.</summary>
        /// <value>The client secret.</value>
        public string ClientSecret { get; set; }

        /// <summary>Gets or sets the callback path.</summary>
        /// <value>The callback path.</value>
        public string CallbackPath { get; set; }

        /// <summary>Gets or sets the signed out callback path.</summary>
        /// <value>The signed out callback path.</value>
        public string SignedOutCallbackPath { get; set; }

        /// <summary>Gets or sets the logout path.</summary>
        /// <value>The logout path.</value>
        public string LogoutPath { get; set; }

        /// <summary>Gets or sets the Profile Url</summary>
        /// <value>The Profile Url.</value>
        public string ProfileUrl { get; set; }

        /// <summary>Gets or sets the timeout.</summary>
        /// <value>The timeout.</value>
        public int Timeout { get; set; }

        /// <summary>/// Gets or sets the issuer./// </summary>
        /// <value>/// The issuer./// </value>
        public string Issuer { get; set; }

        /// <summary>Gets or sets the audience.</summary>
        /// <value>The audience.</value>
        public string Audience { get; set; }

        /// <summary>Gets or sets the Api secret.</summary>
        /// <value>The Api secret.</value>
        public string ApiSecret { get; set; }

        /// <summary>Gets or sets the Api Uri.</summary>
        /// <value>The Api Uri.</value>
        public string ApiUri { get; set; }

        /// <summary>Gets or sets the token endpoint.</summary>
        /// <value>The token endpoint.</value>
        public string TokenEndpoint { get; set; }

        /// <summary>Gets or sets the authority.</summary>
        /// <value>The authority.</value>
        public string Authority { get; set; }

        /// <summary> Gets or sets a value indicating whether [sign out enabled].</summary>
        /// <value><c>true</c> if [sign out enabled]; otherwise, <c>false</c>.</value>
        public bool SignOutEnabled { get; set; }

        /// <summary>Gets or sets a value indicating whether [sign out redirect URI enabled].</summary>
        /// <value><c>true</c> if [sign out redirect URI enabled]; otherwise, <c>false</c>.</value>
        public bool SignOutRedirectUriEnabled { get; set; }

        /// <summary>Gets or sets the sign out redirect URI.</summary>
        /// <value>The sign out redirect URI.</value>
        public string SignOutRedirectUri { get; set; }

        public string TimeoutRedirectUri { get; set; }
    }
}
