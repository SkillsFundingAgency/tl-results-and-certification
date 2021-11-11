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
        private readonly ResultsAndCertificationConfiguration _configuration;

        public UcasApiClient(HttpClient httpClient, ResultsAndCertificationConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiUri = configuration?.UcasApiSettings?.Uri?.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_apiUri);
        }
        string FormatRequestUri(string path) => $"{string.Format(ApiConstants.UcasBaseUri, _configuration?.UcasApiSettings?.Version)}{path}";

        public async Task<string> GetTokenAsync()
        {
            var requestUri = FormatRequestUri(ApiConstants.UcasTokenUri);
            string requestParameters = string.Format(ApiConstants.UcasTokenParameters, _configuration.UcasApiSettings.GrantType,_configuration.UcasApiSettings.Username,_configuration.UcasApiSettings.Password);
            var tokenResponse = await PostAsync<string, UcasTokenResponse>(requestUri, requestParameters);
            if (string.IsNullOrWhiteSpace(tokenResponse?.AccessToken))
                throw new ApplicationException($"Ucas - Failed to retrive api token. Error = {tokenResponse.Error}; ErrorMessage = {tokenResponse.ErrorDescription}");
            return tokenResponse?.AccessToken;
        }

        public async Task<string> SendDataAsync(UcasDataRequest request)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());
            
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(ApiConstants.SHA256), ApiConstants.FormDataHashType);
                content.Add(new StringContent(request.FileHash), ApiConstants.FormDataHash);
                content.Add(new StreamContent(new MemoryStream(request.FileData))
                {
                    Headers =
                    {                        
                        ContentType = new MediaTypeHeaderValue("multipart/form-data")
                    }
                }, ApiConstants.FormDataFile, request.FileName);

                var requestUri = FormatRequestUri(string.Format(ApiConstants.UcasFileUri, _configuration.UcasApiSettings.FolderId));
                var response = await PostAsync<MultipartFormDataContent, UcasDataResponse>(requestUri, content);

                if (string.IsNullOrWhiteSpace(response.Id))
                    throw new ApplicationException($"Ucas - Failed to send data. Error Response : {JsonConvert.SerializeObject(response)}");

                return response.Id;
            }
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent(content));
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
            if (content is MultipartFormDataContent)
                return content as HttpContent;
            else
            {
                var json = content is string ? content.ToString() : JsonConvert.SerializeObject(content);
                return new StringContent(json, Encoding.UTF8, "application/json");
            }
        }
    }
}
