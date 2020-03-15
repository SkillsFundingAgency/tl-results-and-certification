using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Services.AppAuthentication;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class TokenServiceClient : ITokenServiceClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ResultsAndCertificationConfiguration _config;

        public TokenServiceClient(IHttpContextAccessor httpContextAccessor, ResultsAndCertificationConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = configuration;
        }

        public async Task<string> GetToken()
        {
            return await new AzureServiceTokenProvider().GetAccessTokenAsync(_config.ResultsAndCertificationInternalApiSettings.IdentifierUri);
        }
    }
}
