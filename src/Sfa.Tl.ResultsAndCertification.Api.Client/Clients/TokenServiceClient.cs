using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Azure.Services.AppAuthentication;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class TokenServiceClient : ITokenServiceClient
    {
        private readonly ResultsAndCertificationConfiguration _config;

        public TokenServiceClient(ResultsAndCertificationConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<string> GetInternalApiToken()
        {
            return await new AzureServiceTokenProvider().GetAccessTokenAsync(_config.ResultsAndCertificationInternalApiSettings.IdentifierUri);
        }

        public string GetDfeApiToken()
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .Issuer(_config.DfeSignInSettings.Issuer)
                .Audience(_config.DfeSignInSettings.Audience)
                .WithSecret(_config.DfeSignInSettings.ApiSecret)
                .Encode();

            return token;
        }
    }
}
