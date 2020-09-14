using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
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
    public class When_VerifyTlevel_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected readonly long ukprn = 1024;
        protected bool _result;

        protected readonly string RouteName = "Construction";
        protected readonly string PathwayName = "Design, Surveying and Planning";
        protected readonly int StatusId = 1;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected bool _mockHttpResult;

        private VerifyTlevelDetails _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = true;
            _model = new VerifyTlevelDetails
            {
                TqAwardingOrganisationId = 1,
                PathwayStatusId = 2,
                ModifiedBy = "Test User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<bool>(_mockHttpResult, ApiConstants.VerifyTlevelUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.VerifyTlevelAsync(_model);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();
        }
    }
}
