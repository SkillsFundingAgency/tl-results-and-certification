using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetLoggedInUserTypeInfo_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private long _ukprn;
        protected LoggedInUserTypeInfo _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private LoggedInUserTypeInfo _result;

        public override void Setup()
        {
            _ukprn = 987654321;
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new LoggedInUserTypeInfo
            {
                Ukprn = _ukprn,
                Name = "Test",
                UserType = LoginUserType.AwardingOrganisation
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<LoggedInUserTypeInfo>(_mockHttpResult, string.Format(ApiConstants.GetLoggedInUserTypeInfoUri, _ukprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetLoggedInUserTypeInfoAsync(_ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Ukprn.Should().Be(_mockHttpResult.Ukprn);
            _result.Name.Should().Be(_mockHttpResult.Name);
            _result.UserType.Should().Be(_mockHttpResult.UserType);
        }
    }
}
