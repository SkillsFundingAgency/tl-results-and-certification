using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class DataProtectionExtensions
    {
        public static IServiceCollection AddWebDataProtection(this IServiceCollection services, ResultsAndCertificationConfiguration config, AzureServiceTokenProvider tokenProvider, IWebHostEnvironment env)
        {
            var dataProtection = services.AddDataProtection();

            if (!env.IsDevelopment())
            {
                var kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
                dataProtection.PersistKeysToAzureBlobStorage(GetDataProtectionBlobToken(config))
                    .ProtectKeysWithAzureKeyVault(kvClient, config.DataProtectionSettings.KeyVaultKeyId);
            }            
            return services;
        }

        private static Uri GetDataProtectionBlobToken(ResultsAndCertificationConfiguration config)
        {
            var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(config.BlobStorageSettings.AccountName, config.BlobStorageSettings.AccountKey), useHttps: true);
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(config.DataProtectionSettings.ContainerName);
            var blob = container.GetBlockBlobReference(config.DataProtectionSettings.BlobName);
            
            var sharedAccessPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
            };

            var sasToken = blob.GetSharedAccessSignature(sharedAccessPolicy);
            return new Uri(blob.Uri + sasToken);
        }
    }
}
