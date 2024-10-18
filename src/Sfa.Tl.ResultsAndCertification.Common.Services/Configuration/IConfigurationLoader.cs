using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Configuration
{
    public interface IConfigurationLoader
    {
        ResultsAndCertificationConfiguration Load(string environment, string version, string serviceName);
    }
}