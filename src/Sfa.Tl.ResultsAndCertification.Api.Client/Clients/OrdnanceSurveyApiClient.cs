using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class OrdnanceSurveyApiClient : IOrdnanceSurveyApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public OrdnanceSurveyApiClient(HttpClient httpClient, ResultsAndCertificationConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.OrdnanceSurveyApiSettings?.PlacesApiBaseUri != null ? new Uri(configuration.OrdnanceSurveyApiSettings.PlacesApiBaseUri.TrimEnd('/')) : null;
        }

        public async Task<PostcodeLookupResult> GetAddressesByPostcode(string postcode)
        {
            var searchResponse = await _httpClient.GetAsync(string.Format(ApiConstants.SearchAddressByPostcodeUri, postcode, _configuration.OrdnanceSurveyApiSettings?.PlacesApiKey));
            searchResponse.EnsureSuccessStatusCode();
            return await searchResponse.Content.ReadAsAsync<PostcodeLookupResult>();
        }
    }
}
