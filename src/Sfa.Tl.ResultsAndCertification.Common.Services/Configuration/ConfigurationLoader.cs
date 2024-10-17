using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Linq;
using HelperConstants = Sfa.Tl.ResultsAndCertification.Common.Helpers.Constants;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Configuration
{
    public class ConfigurationLoader
    {
        public static ResultsAndCertificationConfiguration Load(IConfiguration config)
            => Load(config[HelperConstants.EnvironmentNameConfigKey],
                config[HelperConstants.TableServiceUriConfigKey],
                config[HelperConstants.VersionConfigKey],
                config[HelperConstants.ServiceNameConfigKey]);

        public static ResultsAndCertificationConfiguration Load(string environment, string tableServiceUri, string version, string serviceName)
        {
            try
            {
                var tableClient = new TableClient(new Uri(tableServiceUri), "Configuration", new DefaultAzureCredential());

                var tableEntity = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{environment}' and RowKey eq '{serviceName}_{version}'");
                var data = tableEntity.FirstOrDefault()?["Data"]?.ToString();

                return JsonConvert.DeserializeObject<ResultsAndCertificationConfiguration>(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Configuration could not be loaded. Please check your configuration files or see the inner exception for details", ex);
            }
        }
    }
}