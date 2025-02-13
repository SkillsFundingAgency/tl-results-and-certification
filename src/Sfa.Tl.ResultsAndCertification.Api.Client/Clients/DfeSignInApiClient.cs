using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Collections.Generic;
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
            var organisation = GetOrganisation(organisationId, userId);
            var userInfo = GetUserInfo(organisationId, userId);

            await Task.WhenAll(organisation, userInfo);

            var userInfoResult = userInfo.Result;
            var organisationResult = organisation.Result;
            userInfoResult.Organisation = organisation.Result.Name;

            if (organisationResult.UKPRN.HasValue)
                userInfoResult.Ukprn = organisationResult.UKPRN;
            else userInfoResult.HasAccessToService = HasAccesstoService(Common.Constants.OrganisationConstants.AdminOrganisation, userInfoResult, RolesExtensions.AdminDashboardAccess);

            return userInfoResult;
        }

        public async Task<IEnumerable<DfeUsers>> GetDfeUsersAllProviders(IEnumerable<long> ukPrns)
        {
            var users = new List<DfeUsers>();

            foreach (var ukPrn in ukPrns)
            {
                var response = await GetDfeUsersForProvider(ukPrn.ToString());

                System.Diagnostics.Debug.WriteLine($"ukPrn: {ukPrn}, Users: {response?.Users?.Count()}");

                response?.Users?.ToList().ForEach(e =>
                    System.Diagnostics.Debug.WriteLine($"Name: {e?.FirstName} {e?.LastName}, Email: {e?.Email}, Status: {e?.UserStatus}"));

                if (response.Users != null && response.Users.Any())
                {
                    users.Add(response);
                }
            }

            return GetActiveUsers(users);
        }

        public async Task<DfeUsers> GetDfeUsersForProvider(string ukPrn)
        {
            var users = new DfeUsers();
            var requestUri = $"/organisations/{ukPrn}/users";
            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<DfeUsers>(responseContent);
            }
            return users;
        }

        private async Task<Organisation> GetOrganisation(string organisationId, string userId)
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

        private bool HasAccesstoService(string organisation, DfeUserInfo userInfo, string role)
        {
            return userInfo.Roles.Any(t => t.Name == role) && userInfo.Organisation == organisation;
        }

        private List<DfeUsers> GetActiveUsers(List<DfeUsers> users)
           => users
              .Where(u => u.Users.Any(user => user.UserStatus == DfeUserStatus.Active))
              .Select(o => new DfeUsers
              {
                  Ukprn = o.Ukprn,
                  Users = o.Users.Where(user => user.UserStatus == DfeUserStatus.Active)
              })
              .ToList();
    }
}
