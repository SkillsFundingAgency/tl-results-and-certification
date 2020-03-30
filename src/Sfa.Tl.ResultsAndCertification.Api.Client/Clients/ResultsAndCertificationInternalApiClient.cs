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
        private readonly bool _isDevevelopment;

        public ResultsAndCertificationInternalApiClient(HttpClient httpClient, ITokenServiceClient tokenService, ResultsAndCertificationConfiguration configuration)
        {
            _isDevevelopment = configuration.IsDevevelopment;
            _tokenServiceClient = tokenService;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _internalApiUri = configuration.ResultsAndCertificationInternalApiSettings.Uri.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_internalApiUri);
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            return await GetAsync<IEnumerable<AwardingOrganisationPathwayStatus>>(string.Format(ApiConstants.GetAllTLevelsUri, ukprn));
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            return await GetAsync<IEnumerable<AwardingOrganisationPathwayStatus>>(string.Format(ApiConstants.GetTlevelsByStatus, ukprn, statusId));
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var requestUri = string.Format(ApiConstants.TlevelDetailsUri, ukprn, id);
            return await GetAsync<TlevelPathwayDetails>(requestUri);
        }

        public async Task<bool> VerifyTlevelAsync(VerifyTlevelDetails model)
        {
            var requestUri = ApiConstants.VerifyTlevelUri;
            return await PutAsync<VerifyTlevelDetails, bool>(requestUri, model);
        }

        public async Task<bool> AddProviderTlevelsAsync(IList<ProviderTlevelDetails> model)
        {
            var requestUri = ApiConstants.AddProviderTlevelsUri;
            return await PostAsync<IList<ProviderTlevelDetails>, bool>(requestUri, model);
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            var requestUri = string.Format(ApiConstants.IsAnyProviderSetupCompletedUri, ukprn);
            return await GetAsync<bool>(requestUri);
        }

        public async Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch)
        {
            var requestUri = string.Format(ApiConstants.FindProviderAsyncUri, name, isExactMatch);
            return await GetAsync<IEnumerable<ProviderMetadata>>(requestUri);
        }

        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var requestUri = string.Format(ApiConstants.GetSelectProviderTlevelsUri, aoUkprn, providerId);
            return await GetAsync<ProviderTlevels>(requestUri);
        }

        public async Task<ProviderTlevels> GetProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var requestUri = string.Format(ApiConstants.GeAlltProviderTlevelsAsyncUri, aoUkprn, providerId);
            return await GetAsync<ProviderTlevels>(requestUri);
        }

        public async Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn)
        {
            var requestUri = string.Format(ApiConstants.GetTqAoProviderDetailsAsyncUri, aoUkprn);
            return await GetAsync<IList<ProviderDetails>>(requestUri);
        }

        private async Task SetBearerToken()
        {
            if (!_isDevevelopment)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenServiceClient.GetToken());
            }
        }

        private async Task<T> GetAsync<T>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsAsync<T>();
            return data;
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            await SetBearerToken();
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent<TRequest>(content));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();
        }

        private async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            await SetBearerToken();
            var response = await _httpClient.PutAsync(requestUri, CreateHttpContent<TRequest>(content));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>();
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
