using Azure.Storage.Blobs;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class DataProtectionExtensions
    {
        public static IServiceCollection AddWebDataProtection(this IServiceCollection services, ResultsAndCertificationConfiguration config, IWebHostEnvironment env)
        {
            BlobClient blobClient = GetBlobClient(config, isDevelopment: env.IsDevelopment());

            services.AddDataProtection()
                    .PersistKeysToAzureBlobStorage(blobClient);

            return services;
        }

        private static BlobClient GetBlobClient(ResultsAndCertificationConfiguration config, bool isDevelopment)
        {
            DataProtectionSettings settings = config.DataProtectionSettings;

            var factory = new BlobClientFactory(config.BlobContainerUriTemplate, isDevelopment);
            return factory.Create(settings.ContainerName, settings.BlobName);
        }
    }
}