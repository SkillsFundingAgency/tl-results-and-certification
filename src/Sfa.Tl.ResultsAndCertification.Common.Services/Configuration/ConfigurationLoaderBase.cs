using Azure.Data.Tables;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Configuration
{
    public abstract class ConfigurationLoaderBase
    {
        protected const string TableName = "Configuration";
        private readonly Func<TableClient> _getTableClient;

        public ConfigurationLoaderBase(Func<TableClient> getTableClient)
        {
            _getTableClient = getTableClient;
        }

        public ResultsAndCertificationConfiguration Load(string environment, string version, string serviceName)
        {
            try
            {
                var tableClient = _getTableClient();

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