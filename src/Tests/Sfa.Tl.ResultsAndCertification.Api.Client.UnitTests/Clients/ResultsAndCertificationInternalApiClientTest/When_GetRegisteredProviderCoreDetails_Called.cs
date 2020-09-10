using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetRegisteredProviderCoreDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private Task<IList<PathwayDetails>> _result;
        private readonly long _ukprn = 12345678;
        private readonly long _providerUkprn = 987654321;
        protected IList<PathwayDetails> _mockHttpResult;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<PathwayDetails>
            {
                new PathwayDetails
                {
                    Id = 1,
                    Name = "Test",
                    Code = "10000111"
                },
                new PathwayDetails
                {
                    Id = 2,
                    Name = "Display",
                    Code = "10000112"
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IList<PathwayDetails>>(_mockHttpResult, string.Format(ApiConstants.GetRegisteredProviderPathwayDetailsAsyncUri, _ukprn, _providerUkprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetRegisteredProviderPathwayDetailsAsync(_ukprn, _providerUkprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();
            actualResult.Count().Should().Be(2);
            var expectedCoreDetailsResult = _mockHttpResult.FirstOrDefault();
            var actualCoreDetailsResult = actualResult.FirstOrDefault();
            actualCoreDetailsResult.Should().NotBeNull();

            actualCoreDetailsResult.Id.Should().Be(expectedCoreDetailsResult.Id);
            actualCoreDetailsResult.Name.Should().Be(expectedCoreDetailsResult.Name);
            actualCoreDetailsResult.Code.Should().Be(expectedCoreDetailsResult.Code);
        }
    }
}
