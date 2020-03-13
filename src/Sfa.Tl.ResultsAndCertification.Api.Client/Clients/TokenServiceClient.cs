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

        public string GetToken()
        {
            var userClaims = _httpContextAccessor.HttpContext.User.Claims;

            var roleClaims = new List<Claim>();
            if (userClaims != null)
            {
                roleClaims = userClaims.Where(c => c.Type == ClaimTypes.Role).ToList();
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.ResultsAndCertificationInternalApiSettings.InternalApiSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config.ResultsAndCertificationInternalApiSettings.InternalApiIssuer,
                Subject = new ClaimsIdentity(roleClaims),
                Expires = DateTime.UtcNow.AddSeconds(_config.ResultsAndCertificationInternalApiSettings.InternalApiTokenExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
