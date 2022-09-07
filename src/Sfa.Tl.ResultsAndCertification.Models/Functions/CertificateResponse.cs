namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class CertificateResponse : FunctionResponse
    {
        /// <summary>
        /// Gets or sets the batch identifier.
        /// </summary>
        /// <value>
        /// The batch identifier.
        /// </value>
        public int BatchId { get; set; }

        /// <summary>
        /// Gets or sets the providers count.
        /// </summary>
        /// <value>
        /// The providers count.
        /// </value>
        public int ProvidersCount { get; set; }        

        public int CertificatesCreated { get; set; }
        
        public int OverallResultsUpdatedCount { get; set; }
    }
}
