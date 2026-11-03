using Azure.Core;
using Azure.Identity;
using JWT.Algorithms;
using JWT.Builder;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class TokenServiceClient : ITokenServiceClient
    {
        private static readonly TokenCredential Credential = new DefaultAzureCredential();
        private readonly ResultsAndCertificationConfiguration _config;

        public TokenServiceClient(ResultsAndCertificationConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<string> GetInternalApiToken()
        {
            var resource = _config.ResultsAndCertificationInternalApiSettings.IdentifierUri.TrimEnd('/');
            var token = await Credential.GetTokenAsync(new TokenRequestContext(new[] { $"{resource}/.default" }), default);
            return token.Token;
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
