using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class UcasApiClient : IUcasApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUri;
        private ResultsAndCertificationConfiguration _configuration;

        public UcasApiClient(HttpClient httpClient, ResultsAndCertificationConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiUri = string.Format(ApiConstants.UcasBaseUri, configuration?.UcasApiSettings?.Uri?.TrimEnd('/'), configuration?.UcasApiSettings?.Version);
            _httpClient.BaseAddress = new Uri(_apiUri);
        }

        public async Task<string> GetTokenAsync()
        {
            var requestUri = string.Format(ApiConstants.UcasTokenUri, _configuration.UcasApiSettings.Version);
            string requestParameters = string.Format(ApiConstants.UcasTokenParameters, _configuration.UcasApiSettings.GrantType,_configuration.UcasApiSettings.GrantType,_configuration.UcasApiSettings.GrantType);

            var tokenResponse = await PostAsync<string, UcasTokenResponse>(requestUri, requestParameters);
            return tokenResponse?.AccessToken;
        }

        public async Task<bool> SendData(string fileName, byte[] data)
        {
            var requestUri = string.Format(ApiConstants.UcasFileUri, _configuration.UcasApiSettings.FolderId);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(new MemoryStream(data))
                {
                    Headers =
                    {
                        ContentLength = data.Length,
                        ContentType = new MediaTypeHeaderValue("multipart/form-data")
                    }
                }, "file", fileName);

                var response = await PostAsync<MultipartFormDataContent, UcasDataResponse>(requestUri, content);
                return !string.IsNullOrWhiteSpace(response.Id);               
            }            
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent(content));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = content is string ? content.ToString() : JsonConvert.SerializeObject(content);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
