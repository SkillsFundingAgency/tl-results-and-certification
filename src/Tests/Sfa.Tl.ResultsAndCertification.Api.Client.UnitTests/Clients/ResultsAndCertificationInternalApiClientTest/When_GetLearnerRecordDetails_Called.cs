using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetLearnerRecordDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // inputs
        private readonly long _providerUkprn = 12345678;
        private readonly int _profileId = 7;
        private readonly int _pathwayId = 10;
        private readonly long _uln = 987654321;

        // results
        private LearnerRecordDetails _actualResult;
        private LearnerRecordDetails _mockApiResponse;

        // dependencies
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

            _mockApiResponse = new LearnerRecordDetails
            {
                ProfileId = _profileId,
                Uln = _uln,
                Name = "Test User",
                DateofBirth = DateTime.UtcNow.AddYears(30),
                ProviderName = "Barnsley College (123456789)",
                IsLearnerRegistered = true,
                IndustryPlacementId = 7,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<LearnerRecordDetails>(_mockApiResponse, string.Format(ApiConstants.GetLearnerRecordDetailsUri, _providerUkprn, _profileId, _pathwayId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetLearnerRecordDetailsAsync(_providerUkprn, _profileId, _pathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.ProfileId.Should().Be(_mockApiResponse.ProfileId);
            _actualResult.Uln.Should().Be(_mockApiResponse.Uln);
            _actualResult.Name.Should().Be(_mockApiResponse.Name);
            _actualResult.DateofBirth.Should().Be(_mockApiResponse.DateofBirth);
            _actualResult.ProviderName.Should().Be(_mockApiResponse.ProviderName);
            _actualResult.IsLearnerRegistered.Should().Be(_mockApiResponse.IsLearnerRegistered);
            _actualResult.IndustryPlacementId.Should().Be(_mockApiResponse.IndustryPlacementId);
            _actualResult.IndustryPlacementStatus.Should().Be(_mockApiResponse.IndustryPlacementStatus);
        }
    }
}
