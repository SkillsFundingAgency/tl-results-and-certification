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
        /// Gets or sets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        public string SqlConnectionString { get; set; }

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
        public bool IsDev { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable local authentication].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable local authentication]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableLocalAuthentication { get; set; }
    }
}
