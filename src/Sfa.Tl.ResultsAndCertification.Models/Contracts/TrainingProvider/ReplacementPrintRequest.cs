namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class ReplacementPrintRequest
    {
        /// <summary>
        /// Gets or sets the uln.
        /// </summary>
        /// <value>
        /// The uln.
        /// </value>
        public long Uln { get; set; }

        /// <summary>
        /// Gets or sets the provider ukprn.
        /// </summary>
        /// <value>
        /// The provider ukprn.
        /// </value>
        public long ProviderUkprn { get; set; }

        /// <summary>
        /// Gets or sets the provider address identifier.
        /// </summary>
        /// <value>
        /// The provider address identifier.
        /// </value>
        public int ProviderAddressId { get; set; }

        /// <summary>
        /// Gets or sets the print certificate identifier.
        /// </summary>
        /// <value>
        /// The print certificate identifier.
        /// </value>
        public int PrintCertificateId { get; set; }

        /// <summary>
        /// Gets or sets the performed by.
        /// </summary>
        /// <value>
        /// The performed by.
        /// </value>
        public string PerformedBy { get; set; }
    }
}
