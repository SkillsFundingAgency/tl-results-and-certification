using Azure.Data.Tables;
using Azure.Identity;
using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Configuration
{
    public class ConfigurationLoader : ConfigurationLoaderBase, IConfigurationLoader
    {
        public ConfigurationLoader(string tableServiceUri)
            : base(() => GetTableClient(tableServiceUri))
        {
        }

        private static TableClient GetTableClient(string tableServiceUri)
            => new(new Uri(tableServiceUri), TableName, new DefaultAzureCredential());
    }
}