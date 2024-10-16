using Azure.Storage.Blobs;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication
{
    public static class DataProtectionExtensions
    {
        public static IServiceCollection AddWebDataProtection(this IServiceCollection services, ResultsAndCertificationConfiguration config)
        {
            BlobClient blobClient = GetBlobClient(config);

            services.AddDataProtection()
                    .PersistKeysToAzureBlobStorage(blobClient);

            return services;
        }

        private static BlobClient GetBlobClient(ResultsAndCertificationConfiguration config)
        {
            DataProtectionSettings settings = config.DataProtectionSettings;

            var factory = new BlobClientFactory(config);
            return factory.Create(settings.ContainerName, settings.BlobName);
        }
    }
}