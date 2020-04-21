using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, ResultsAndCertificationConfiguration configuration)
        {
            // configure jwt authentication
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Authority = $"https://login.microsoftonline.com/{configuration.ResultsAndCertificationInternalApiSettings.TenantId}";
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = new List<string>
                    {
                        configuration.ResultsAndCertificationInternalApiSettings.IdentifierUri,
                        configuration.ResultsAndCertificationInternalApiSettings.ClientId
                    }
                };
            });
            return services;
        }

        public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole("Application")
                    .Build();

                options.DefaultPolicy = policy;
            });
            return services;
        }

        public static IServiceCollection AddApiDataProtection(this IServiceCollection services, ResultsAndCertificationConfiguration config, AzureServiceTokenProvider tokenProvider, IWebHostEnvironment env)
        {
            var dataProtection = services.AddDataProtection();

            if (env.IsDevelopment())
            {
                dataProtection.PersistKeysToFileSystem(new DirectoryInfo(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "keys")))
                              .SetApplicationName("ResultsAndCertification");
            }
            else
            {
                var kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
                dataProtection.PersistKeysToAzureBlobStorage(GetDataProtectionBlobTokenUri(config))
                              .ProtectKeysWithAzureKeyVault(kvClient, config.DataProtectionSettings.KeyVaultKeyId);
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
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
            };

            var sasToken = blob.GetSharedAccessSignature(sharedAccessPolicy);
            return new Uri($"{blob.Uri}{sasToken}");
        }
    }
}
