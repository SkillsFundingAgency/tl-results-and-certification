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
    public class When_GetAvailableAssessmentSeries_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        private readonly int _profileId = 1;
        private readonly ComponentType componentType = ComponentType.Core;
        private readonly string componentIds;
        protected AvailableAssessmentSeries _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private AvailableAssessmentSeries _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new AvailableAssessmentSeries
            {
                ProfileId = _profileId,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AvailableAssessmentSeries>(_mockHttpResult, string.Format(ApiConstants.GetAvailableAssessmentSeriesUri, _ukprn, _profileId, (int)componentType, componentIds), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAvailableAssessmentSeriesAsync(_ukprn, _profileId, componentType, componentIds);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.ProfileId.Should().Be(_mockHttpResult.ProfileId);
            _result.AssessmentSeriesId.Should().Be(_mockHttpResult.AssessmentSeriesId);
            _result.AssessmentSeriesName.Should().Be(_mockHttpResult.AssessmentSeriesName);
        }
    }
}
