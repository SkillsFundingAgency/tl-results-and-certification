using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Certificates
{
    public class KeyVaultCertificateService
    {
        private readonly string _keyVaultUri;
        private readonly string _certificateName;

        public KeyVaultCertificateService(string keyVaultUri, string certificateName)
        {
            _keyVaultUri = keyVaultUri;
            _certificateName = certificateName;
        }

        public async Task<X509Certificate2> GetCertificateFromKeyVault()
        {                           
            try
            {
                var defaultAzureCredentialOptions = new DefaultAzureCredentialOptions();
#if DEBUG
                defaultAzureCredentialOptions = new DefaultAzureCredentialOptions { VisualStudioTenantId = "9c7d9dd3-840c-4b3f-818e-552865082e16" };
#endif      
                var certificateClient = new CertificateClient(vaultUri: new Uri(_keyVaultUri), credential: new DefaultAzureCredential(defaultAzureCredentialOptions));
                var certificate = await certificateClient.DownloadCertificateAsync(_certificateName);
                return certificate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
