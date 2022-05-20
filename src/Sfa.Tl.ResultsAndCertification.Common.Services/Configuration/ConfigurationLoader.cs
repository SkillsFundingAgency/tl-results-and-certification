using Azure.Data.Tables;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Configuration
{
    public class ConfigurationLoader
    {
        public static ResultsAndCertificationConfiguration Load(string environment, string storageConnectionString,
            string version, string serviceName)
        {
            try
            {
                var tableClient = new TableClient(storageConnectionString, "Configuration");
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