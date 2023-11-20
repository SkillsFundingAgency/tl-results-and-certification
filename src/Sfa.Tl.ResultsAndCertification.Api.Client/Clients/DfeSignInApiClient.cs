using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.ComponentModel;
using System.Linq;
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

        public async Task<DfeUserInfo> GetDfeSignInUserInfo(string organisationId, string userId)
        {
            var organisationUkprn = GetOrganisationUkprn(organisationId, userId);
            var userInfo = GetUserInfo(organisationId, userId);

            await Task.WhenAll(organisationUkprn, userInfo);

            var userInfoResult = userInfo.Result;
            var ukprn = organisationUkprn.Result;
            userInfoResult.Organisation = organisationUkprn.Result.Name;

            if (ukprn.UKPRN.HasValue)
                userInfoResult.Ukprn = ukprn.UKPRN;
            else if (!ukprn.UKPRN.HasValue) userInfoResult.HasAccessToService = HasAccesstoService(Common.Constants.OrganisationConstants.AdminOrganisation, userInfoResult, RolesExtensions.AdminDashboardAccess);
            else
                userInfoResult.HasAccessToService = false;
            
            return userInfoResult;
        }

        private async Task<Organisation> GetOrganisationUkprn(string organisationId, string userId)
        {
            var organisation = new Organisation();
            var requestUri = $"/users/{userId}/organisations";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var orgToken = JArray.Parse(responseContent).FirstOrDefault(org => org.SelectToken("id").ToString() == organisationId);
                organisation.UKPRN = (int?)(orgToken?["ukprn"]);
                organisation.Name = (string)(orgToken?["name"]);
                return organisation;
            }            
            return organisation;
        }

        private async Task<DfeUserInfo> GetUserInfo(string organisationId, string userId)
        {
            var userClaims = new DfeUserInfo();
            var requestUri = $"/services/{_clientId}/organisations/{organisationId}/users/{userId}";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                userClaims = JsonConvert.DeserializeObject<DfeUserInfo>(responseContent);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                userClaims.HasAccessToService = false;
            }
            return userClaims;
        }

        private bool HasAccesstoService(string organisation, DfeUserInfo userInfo,string role)
        {
            return userInfo.Roles.Any(t => t.Name == role) && userInfo.Organisation == organisation;
        }

    }
}
