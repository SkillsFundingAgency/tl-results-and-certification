
namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class ResultsAndCertificationConfiguration
    {
        public string BlobStorageConnectionString { get; set; }

        public string SqlConnectionString { get; set; }

        public DfeSignInSettings DfeSignInSettings { get; set; }

        public string ResultsAndCertificationInternalApiUri { get; set; }
    }
}
