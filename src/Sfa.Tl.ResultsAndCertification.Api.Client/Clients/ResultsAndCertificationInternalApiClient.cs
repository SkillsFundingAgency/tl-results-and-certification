using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class ResultsAndCertificationInternalApiClient : IResultsAndCertificationInternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _internalApiUri;
        private readonly ITokenServiceClient _tokenServiceClient;

        public ResultsAndCertificationInternalApiClient(HttpClient httpClient, ITokenServiceClient tokenService, ResultsAndCertificationConfiguration configuration)
        {
            _tokenServiceClient = tokenService;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _internalApiUri = configuration.ResultsAndCertificationApiSettings.InternalApiUri.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_internalApiUri);
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            return await GetAsync<IEnumerable<AwardingOrganisationPathwayStatus>>(string.Format(ApiConstants.GetAllTLevelsUri, ukprn));
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var requestUri = string.Format(ApiConstants.TlevelDetailsUri, ukprn, id);
            return await GetAsync<TlevelPathwayDetails>(requestUri);
        }

        private void SetBearerToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenServiceClient.GetToken());
        }

        private async Task<T> GetAsync<T>(string requestUri)
        {
            SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsAsync<T>();
            return data;
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        private async Task<T> PostAsync<T>(string requestUri, T content)
        {
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsAsync<T>();
            return data;
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
    }
}
