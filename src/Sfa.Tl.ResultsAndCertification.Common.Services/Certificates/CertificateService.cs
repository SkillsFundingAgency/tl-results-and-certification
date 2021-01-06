using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Certificates
{
    public static class CertificateService
    {
        public static async Task<X509Certificate2> GetLearningRecordServiceCertificate(ResultsAndCertificationConfiguration config)
        {
            X509Certificate2 certificate = null;
            if (!string.IsNullOrEmpty(config.KeyVaultUri))
            {
                var keyVaultCertificateService = new KeyVaultCertificateService(config.KeyVaultUri, config.LearningRecordServiceSettings.CertificateName);
                certificate = await keyVaultCertificateService.GetCertificateFromKeyVault();
            }
            else
            {
                using X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var storeCertificates = store.Certificates.Find(X509FindType.FindBySubjectName, "cmp-ws.lrs.education.gov.uk", false);
                certificate = storeCertificates[0];
                store.Close();
            }
            return certificate;
        }
    }
}
