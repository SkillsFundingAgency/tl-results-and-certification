namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class DataProtectionSettings
    {
        /// <summary>
        /// Gets or sets the name of the container.
        /// </summary>
        /// <value>
        /// The name of the container.
        /// </value>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the BLOB.
        /// </summary>
        /// <value>
        /// The name of the BLOB.
        /// </value>
        public string BlobName { get; set; }

        /// <summary>
        /// Gets or sets the key vault key identifier.
        /// </summary>
        /// <value>
        /// The key vault key identifier.
        /// </value>
        public string KeyVaultKeyId { get; set; }
    }
}
