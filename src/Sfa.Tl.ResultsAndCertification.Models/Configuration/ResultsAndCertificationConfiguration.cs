namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class ResultsAndCertificationConfiguration
    {
        /// <summary>
        /// Gets or sets the BLOB storage connection string.
        /// </summary>
        /// <value>
        /// The BLOB storage connection string.
        /// </value>
        public string BlobStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the Blob storage accout name.
        /// </summary>
        /// <value>
        /// The BLOB storage account name.
        /// </value>
        public string BlobStorageAccountName { get; set; }

        /// <summary>
        /// Gets or sets the Blob storage data protection uri storage container name.
        /// </summary>
        /// <value>
        /// The BLOB storage data protection container.
        /// </value>
        public string BlobStorageDataProtectionContainer { get; set; }

        /// <summary>
        /// Gets or sets the Blob storage data protection blob.
        /// </summary>
        /// <value>
        /// The BLOB storage data protection blob.
        /// </value>
        public string BlobStorageDataProtectionBlob { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        public string SqlConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the gov uk notify API key.
        /// </summary>
        /// <value>
        /// The gov uk notify API key.
        /// </value>
        public string GovUkNotifyApiKey { get; set; }

        /// <summary>
        /// Gets or sets the tlevel queried support email address.
        /// </summary>
        /// <value>
        /// The tlevel queried support email address.
        /// </value>
        public string TlevelQueriedSupportEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the dfe sign in settings.
        /// </summary>
        /// <value>
        /// The dfe sign in settings.
        /// </value>
        public DfeSignInSettings DfeSignInSettings { get; set; }

        /// <summary>
        /// Gets or sets the results and certification Internal API settings.
        /// </summary>
        /// <value>
        /// The results and certification Internal API settings.
        /// </value>
        public ResultsAndCertificationInternalApiSettings ResultsAndCertificationInternalApiSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dev.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dev; otherwise, <c>false</c>.
        /// </value>
        public bool IsDevevelopment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable local authentication].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable local authentication]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableLocalAuthentication { get; set; }
    }
}
