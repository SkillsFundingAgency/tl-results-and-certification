using Azure.Storage.Sas;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
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
            var factory = new BlobContainerClientFactory(config);
            var blobContainerClient = factory.Create(config.DataProtectionSettings.ContainerName);
          
            var blobClient = blobContainerClient.GetBlobClient(config.DataProtectionSettings.BlobName?.ToLowerInvariant());
            var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read | BlobSasPermissions.Write | BlobSasPermissions.Create, DateTime.UtcNow.AddYears(1));

            return sasUri;
        }
    }
}