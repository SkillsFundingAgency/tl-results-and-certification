using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class TokenServiceClient : ITokenServiceClient
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ResultsAndCertificationConfiguration _config;

        public TokenServiceClient(IHttpContextAccessor httpContextAccessor, ResultsAndCertificationConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = configuration;
        }

        public async Task<string> GetToken()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string token = await azureServiceTokenProvider.GetAccessTokenAsync(_config.ResultsAndCertificationInternalApiSettings.IdentifierUri);
            return token;
        }
    }

}
