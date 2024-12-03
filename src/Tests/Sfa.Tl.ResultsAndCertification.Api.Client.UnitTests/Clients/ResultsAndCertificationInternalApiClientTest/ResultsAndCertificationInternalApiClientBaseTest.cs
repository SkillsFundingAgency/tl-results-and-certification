using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net.Http;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public abstract class ResultsAndCertificationInternalApiClientBaseTest : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly ITokenServiceClient _tokenServiceClient = Substitute.For<ITokenServiceClient>();
        private readonly ResultsAndCertificationConfiguration _configuration = new()
        {
            ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
        };


        public override void Setup()
        {
        }

        protected ResultsAndCertificationInternalApiClient CreateApiClient<T>(Func<MockHttpMessageHandler<T>> getHttpMessageHandler)
        {
            HttpClient = new HttpClient(getHttpMessageHandler());
            return new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }
    }
}
