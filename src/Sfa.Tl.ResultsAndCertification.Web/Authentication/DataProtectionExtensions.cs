using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class DataProtectionExtensions
    {
        public static IServiceCollection AddWebDataProtection(this IServiceCollection services, AzureServiceTokenProvider tokenProvider, ResultsAndCertificationConfiguration config, IWebHostEnvironment env)
        {
            var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(config.BlobStorageAccountName, config.BlobStorageAccountKey), useHttps: true);
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(config.BlobStorageDataProtectionContainer);
            var blob = container.GetBlockBlobReference(config.BlobStorageDataProtectionBlob);

            var sharedAccessPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
            };

            var sasToken = blob.GetSharedAccessSignature(sharedAccessPolicy);

            var kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));

            services.AddDataProtection()
                .PersistKeysToAzureBlobStorage(new Uri(blob.Uri + sasToken))
                .ProtectKeysWithAzureKeyVault(kvClient, config.DataProtectionKeyVaultKeyId);

            return services;
        }
    }
}
