using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class DataProtectionExtensions
    {
        public static IServiceCollection AddWebDataProtection(this IServiceCollection services, ResultsAndCertificationConfiguration config, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "keys")))
                        .SetApplicationName("ResultsAndCertification");
            }
            else
            {
                services.AddDataProtection()
                        .PersistKeysToAzureBlobStorage(GetDataProtectionBlobTokenUri(config));
            }
            return services;
        }

        private static Uri GetDataProtectionBlobTokenUri(ResultsAndCertificationConfiguration config)
        {
            var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(config.BlobStorageSettings.AccountName, config.BlobStorageSettings.AccountKey), useHttps: true);
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(config.DataProtectionSettings.ContainerName);
            var blob = container.GetBlockBlobReference(config.DataProtectionSettings.BlobName);
            
            var sharedAccessPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
            };

            var sasToken = blob.GetSharedAccessSignature(sharedAccessPolicy);
            return new Uri($"{blob.Uri}{sasToken}");
        }
    }
}
