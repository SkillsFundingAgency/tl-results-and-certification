using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class DfeSignInApiClient : IDfeSignInApiClient
    {
        private readonly string _clientId;
        private readonly string _dfeSignInApiUri;
        private readonly HttpClient _httpClient;
        private readonly ITokenServiceClient _tokenServiceClient;

        public DfeSignInApiClient(HttpClient httpClient, ITokenServiceClient tokenService, ResultsAndCertificationConfiguration configuration)
        {
            _clientId = configuration.DfeSignInSettings.ClientId;
            _tokenServiceClient = tokenService;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenServiceClient.GetDfeApiToken());
            _dfeSignInApiUri = configuration.DfeSignInSettings.ApiUri.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_dfeSignInApiUri);
        }

        public async Task<DfeClaims> GetUserInfo(string organisationId, string userId)
        {
            var userClaims = new DfeClaims();
            var requestUri = $"/services/{_clientId}/organisations/{organisationId}/users/{userId}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                userClaims = JsonConvert.DeserializeObject<DfeClaims>(responseContent);                
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                userClaims.HasAccessToService = false;
            }
            return userClaims;
        }
    }
}
