﻿using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
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
        }

        public async Task<PostcodeLookupResult> GetAddressesByPostcodeAsync(string postcode)
        {
            var searchResponse = await _httpClient.GetAsync(FormatPlacesRequestUri(string.Format(ApiConstants.SearchAddressByPostcodeUri, postcode, _configuration.OrdnanceSurveyApiSettings?.PlacesKey)));
            searchResponse.EnsureSuccessStatusCode();
            return await searchResponse.Content.ReadAsAsync<PostcodeLookupResult>();
        }

        public async Task<PostcodeLookupResult> GetAddressByUprnAsync(long uprn)
        {
            var searchResponse = await _httpClient.GetAsync(FormatPlacesRequestUri(string.Format(ApiConstants.GetAddressByUprnUri, uprn, _configuration.OrdnanceSurveyApiSettings?.PlacesKey)));
            searchResponse.EnsureSuccessStatusCode();
            return await searchResponse.Content.ReadAsAsync<PostcodeLookupResult>();
        }

        private string FormatPlacesRequestUri(string requestUri)
        {
            return !string.IsNullOrWhiteSpace(requestUri) ? $"{_configuration.OrdnanceSurveyApiSettings.PlacesUri.TrimEnd('/')}{requestUri}" : null;
        }
    }
}