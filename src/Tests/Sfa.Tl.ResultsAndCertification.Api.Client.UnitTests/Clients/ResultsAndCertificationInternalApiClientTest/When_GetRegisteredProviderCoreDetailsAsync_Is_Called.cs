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
    public class When_GetRegisteredProviderCoreDetailsAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private Task<IList<CoreDetails>> _result;
        private readonly long _ukprn = 12345678;
        private readonly long _providerUkprn = 987654321;
        protected IList<CoreDetails> _mockHttpResult;
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

            _mockHttpResult = new List<CoreDetails>
            {
                new CoreDetails
                {
                    Id = 1,
                    CoreName = "Test",
                    CoreCode = "10000111"
                },
                new CoreDetails
                {
                    Id = 2,
                    CoreName = "Display",
                    CoreCode = "10000112"
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IList<CoreDetails>>(_mockHttpResult, string.Format(ApiConstants.GetRegisteredProviderCoreDetailsAsyncUri, _ukprn, _providerUkprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetRegisteredProviderCoreDetailsAsync(_ukprn, _providerUkprn);
        }

        [Fact]
        public void Then_Two_Core_Details_Are_Returned()
        {
            _result.Result.Should().NotBeNull();
            _result.Result.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();

            var expectedCoreDetailsResult = _mockHttpResult.FirstOrDefault();
            var actualCoreDetailsResult = actualResult.FirstOrDefault();
            actualCoreDetailsResult.Should().NotBeNull();

            actualCoreDetailsResult.Id.Should().Be(expectedCoreDetailsResult.Id);
            actualCoreDetailsResult.CoreName.Should().Be(expectedCoreDetailsResult.CoreName);
            actualCoreDetailsResult.CoreCode.Should().Be(expectedCoreDetailsResult.CoreCode);
        }
    }
}
