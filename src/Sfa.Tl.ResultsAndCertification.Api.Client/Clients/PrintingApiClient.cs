using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class PrintingApiClient : IPrintingApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _printingApiUri;
        private ResultsAndCertificationConfiguration _configuration;

        public PrintingApiClient(HttpClient httpClient, ResultsAndCertificationConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _printingApiUri = configuration.PrintingApiSettings.Uri.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_printingApiUri);
        }

        public async Task<string> GetTokenAsync()
        {
            var requestUri = string.Format(ApiConstants.PrintingTokenUri, _configuration.PrintingApiSettings.Username, _configuration.PrintingApiSettings.Password);
            var tokenResponse = await GetAsync<string>(requestUri, false);
            var tokenResult = JObject.Parse(tokenResponse);
            return tokenResult.HasValues ? tokenResult.SelectToken("Token")?.ToString() : null;
        }

        public async Task<PrintResponse> ProcessPrintRequestAsync(PrintRequest printRequest)
        {
            var token = await GetTokenAsync();
            var requestUri = string.Format(ApiConstants.PrintRequestUri, token);
            return await PostAsync<PrintRequest, PrintResponse>(requestUri, printRequest);
        }

        public async Task<BatchSummaryResponse> GetBatchSummaryInfoAsync(int batchNumber)
        {
            var token = await GetTokenAsync();
            var requestUri = string.Format(ApiConstants.PrintBatchSummaryRequestUri, batchNumber, token);
            return await GetAsync<BatchSummaryResponse>(requestUri);
        }

        public async Task<TrackBatchResponse> GetTrackBatchInfoAsync(int batchNumber)
        {
            var token = await GetTokenAsync();
            var requestUri = string.Format(ApiConstants.PrintTrackBatchRequestUri, batchNumber, token);
            return await GetAsync<TrackBatchResponse>(requestUri);
        }        

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        private async Task<T> GetAsync<T>(string requestUri, bool deserializeToString = true)
        {            
            var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var result = deserializeToString ? JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()) : await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent(content));
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, IsoDateFormatSettings);
            var result = new StringContent(json, Encoding.UTF8, "application/json");
            return result;
        }

        /// <summary>
        /// Gets the microsoft date format settings.
        /// </summary>
        /// <value>
        /// The microsoft date format settings.
        /// </value>
        private static JsonSerializerSettings IsoDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                };
            }
        }
    }
}
