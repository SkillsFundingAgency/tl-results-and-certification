namespace Sfa.Tl.ResultsAndCertification.Common.Services.Configuration
{
    public static class ConfigurationLoaderFactory
    {
        public static IConfigurationLoader GetConfigurationLoader(string tableServiceUri, bool isDevelopment)
            =>  isDevelopment ? new ConfigurationLoaderLocal() : new ConfigurationLoader(tableServiceUri);
    }
}