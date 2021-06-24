using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetPrsLearnerDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // inputs
        private readonly long _aoUkprn = 12345678;
        private readonly int _profileId = 99;
        private readonly int _assessmentSeriesId = 1;

        // results
        private PrsLearnerDetails _actualResult;
        private PrsLearnerDetails _mockApiResponse;

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

            _mockApiResponse = new PrsLearnerDetails
            {
                ProfileId = _profileId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = System.DateTime.Today.AddYears(-20),
                Status = RegistrationPathwayStatus.Active,

                ProviderName = "Barsely College",
                ProviderUkprn = 9876543210,

                TlevelTitle = "Tlevel in Childcare",
                PathwayName = "Childcare",
                PathwayCode = "12121212",

                PathwayAssessmentId = 1,
                PathwayAssessmentSeries = "Summer 2021",
                PathwayResultId = 99,
                PathwayGrade = "B",
                PathwayGradeLastUpdatedOn = System.DateTime.Today.AddDays(-15),
                PathwayGradeLastUpdatedBy = "Barsley User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PrsLearnerDetails>(_mockApiResponse, string.Format(ApiConstants.GetPrsLearnerDetailsUri, _aoUkprn, _profileId, _assessmentSeriesId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetPrsLearnerDetailsAsync(_aoUkprn, _profileId, _assessmentSeriesId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.ProfileId.Should().Be(_mockApiResponse.ProfileId);
            _actualResult.Uln.Should().Be(_mockApiResponse.Uln);
            _actualResult.Firstname.Should().Be(_mockApiResponse.Firstname);
            _actualResult.Lastname.Should().Be(_mockApiResponse.Lastname);
            _actualResult.DateofBirth.Should().Be(_mockApiResponse.DateofBirth);
            _actualResult.Status.Should().Be(_mockApiResponse.Status);
            _actualResult.ProviderName.Should().Be(_mockApiResponse.ProviderName);
            _actualResult.ProviderUkprn.Should().Be(_mockApiResponse.ProviderUkprn);
            _actualResult.TlevelTitle.Should().Be(_mockApiResponse.TlevelTitle);
            _actualResult.PathwayName.Should().Be(_mockApiResponse.PathwayName);
            _actualResult.PathwayCode.Should().Be(_mockApiResponse.PathwayCode);
            _actualResult.PathwayAssessmentId.Should().Be(_mockApiResponse.PathwayAssessmentId);
            _actualResult.PathwayAssessmentSeries.Should().Be(_mockApiResponse.PathwayAssessmentSeries);
            _actualResult.PathwayResultId.Should().Be(_mockApiResponse.PathwayResultId);
            _actualResult.PathwayGrade.Should().Be(_mockApiResponse.PathwayGrade);
            _actualResult.PathwayGradeLastUpdatedOn.Should().Be(_mockApiResponse.PathwayGradeLastUpdatedOn);
            _actualResult.PathwayGradeLastUpdatedBy.Should().Be(_mockApiResponse.PathwayGradeLastUpdatedBy);
        }
    }
}