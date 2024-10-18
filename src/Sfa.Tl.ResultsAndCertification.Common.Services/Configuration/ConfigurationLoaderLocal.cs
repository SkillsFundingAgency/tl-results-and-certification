using Azure.Data.Tables;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Configuration
{
    public class ConfigurationLoaderLocal : ConfigurationLoaderBase, IConfigurationLoader
    {
        private const string LocalConnectionString = "UseDevelopmentStorage=true;";

        public ConfigurationLoaderLocal()
            : base(() => new TableClient(LocalConnectionString, TableName))
        {
        }
    }
}