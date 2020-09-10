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
    public class When_GetTqAoProviderDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private Task<IList<ProviderDetails>> _result;
        private readonly long _ukprn = 12345678;
        protected IList<ProviderDetails> _mockHttpResult;
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

            _mockHttpResult = new List<ProviderDetails>
            {
                new ProviderDetails
                {
                    Id = 1,
                    DisplayName = "Test",
                    Ukprn = 10000111
                },
                new ProviderDetails
                {
                    Id = 2,
                    DisplayName = "Display",
                    Ukprn = 10000112
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IList<ProviderDetails>>(_mockHttpResult, string.Format(ApiConstants.GetTqAoProviderDetailsAsyncUri, _ukprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetTqAoProviderDetailsAsync(_ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();
            actualResult.Count().Should().Be(2);
            var expectedProviderDetailsResult = _mockHttpResult.FirstOrDefault();
            var actualProviderDetailsResult = actualResult.FirstOrDefault();
            actualProviderDetailsResult.Should().NotBeNull();

            actualProviderDetailsResult.Id.Should().Be(expectedProviderDetailsResult.Id);
            actualProviderDetailsResult.DisplayName.Should().Be(expectedProviderDetailsResult.DisplayName);
            actualProviderDetailsResult.Ukprn.Should().Be(expectedProviderDetailsResult.Ukprn);
        }
    }
}
