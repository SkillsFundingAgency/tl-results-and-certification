using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients
{
    public class MockHttpMessageHandler<T> : DelegatingHandler
    {
        public int NumberOfCalls { get; private set; }

        private readonly Dictionary<string, HttpResponseMessage> _httpResponses = new Dictionary<string, HttpResponseMessage>();

        public MockHttpMessageHandler(T response, string requestUrl, HttpStatusCode statusCode, string requestContent = null)
        {
            AddHttpResponses(response, requestUrl, statusCode, requestContent);            
        }

        public void AddHttpResponses<TResponse>(TResponse response, string requestUrl, HttpStatusCode statusCode, string requestContent = null)
        {
            if (response != null && !string.IsNullOrWhiteSpace(requestUrl))
            {
                var requestKey = requestUrl + requestContent ?? string.Empty;
                _httpResponses.Add(requestKey, new HttpResponseMessage { StatusCode = statusCode, Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json") });
            }
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string responseContent = string.Empty;
            if (request?.Content != null)
                responseContent = await request?.Content.ReadAsStringAsync();

            NumberOfCalls++;

            var requestKey = System.Web.HttpUtility.UrlDecode(request.RequestUri.PathAndQuery) + responseContent;

            if (_httpResponses.ContainsKey(requestKey))
            {
                return _httpResponses[requestKey];
            }
            else
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
            }
        }
    }
}
