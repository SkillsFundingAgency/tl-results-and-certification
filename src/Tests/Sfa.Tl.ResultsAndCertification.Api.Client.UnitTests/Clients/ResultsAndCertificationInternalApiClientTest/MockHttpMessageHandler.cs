using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients
{
    public class MockHttpMessageHandler<T> : HttpMessageHandler
    {
        private readonly T _response;
        private readonly HttpStatusCode _statusCode;

        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }

        public MockHttpMessageHandler(T response, HttpStatusCode statusCode)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            var jsonResponse = JsonConvert.SerializeObject(_response);

            var result = new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(jsonResponse, UnicodeEncoding.UTF8, "application/json")

            };
            return await Task.Run(() => result);
        }
    }
}
