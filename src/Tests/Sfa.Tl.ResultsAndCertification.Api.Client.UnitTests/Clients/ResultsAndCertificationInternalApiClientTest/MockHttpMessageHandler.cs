using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients
{
    public class MockHttpMessageHandler<T> : DelegatingHandler
    {
        private readonly T _response;
        private readonly HttpStatusCode _statusCode;
        private readonly string _requestUrl;

        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }

        public MockHttpMessageHandler(T response, string requestUrl, HttpStatusCode statusCode)
        {
            _response = response;
            _requestUrl = requestUrl;
            _statusCode = statusCode;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage result;

            var reqResponse = await request.Content.ReadAsStringAsync();

            //NumberOfCalls++;
            var jsonResponse = JsonConvert.SerializeObject(_response);

            if (request.RequestUri.AbsolutePath.Equals(_requestUrl))
            {
                result = new HttpResponseMessage
                {
                    StatusCode = _statusCode,
                    Content = new StringContent(jsonResponse, UnicodeEncoding.UTF8, "application/json")
                };
            }
            else
            {
                result = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
            }

            return result;
            //return await Task.Run(() => result);
        }
    }
}
