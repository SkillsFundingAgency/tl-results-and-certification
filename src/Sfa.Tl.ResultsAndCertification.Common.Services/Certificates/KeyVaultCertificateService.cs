using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
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
            var tokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));         

            try
            {
                var certificateItem = await keyVaultClient.GetCertificateAsync(_keyVaultUri, _certificateName);
                var certificateSecretBundle = await keyVaultClient.GetSecretAsync(certificateItem.SecretIdentifier.Identifier);
                var certificate = new X509Certificate2(Convert.FromBase64String(certificateSecretBundle.Value), (string)null);
                return certificate;
            }
            catch (Exception ex)
            {
                // TODO: log exception
            }
            return null;
        }
    }
}
