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
        /// Gets or sets the results and certification API settings.
        /// </summary>
        /// <value>
        /// The results and certification API settings.
        /// </value>
        public ResultsAndCertificationInternalApiSettings ResultsAndCertificationInternalApiSettings { get; set; }
    }
}
