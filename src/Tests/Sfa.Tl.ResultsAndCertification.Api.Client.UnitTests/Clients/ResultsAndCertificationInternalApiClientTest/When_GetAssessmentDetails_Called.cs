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
    public class When_GetAssessmentDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        private readonly int _profileId = 1;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Active;
        protected AssessmentDetails _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private AssessmentDetails _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new AssessmentDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                ProviderUkprn = 1234567,
                ProviderName = "Test Provider",
                PathwayLarId = "7654321",
                PathwayName = "Pathway",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayAssessmentId = 1,
                SpecialismLarId = "2345678",
                SpecialismName = "Specialism1",
                SpecialismAssessmentSeries = "Autumn 2022",
                SpecialismAssessmentId = 25,
                Status = RegistrationPathwayStatus.Active,
                IsCoreEntryEligible = true
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AssessmentDetails>(_mockHttpResult, string.Format(ApiConstants.GetAssessmentDetailsUri, _ukprn, _profileId, (int)_registrationPathwayStatus), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAssessmentDetailsAsync(_ukprn, _profileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Uln.Should().Be(_mockHttpResult.Uln);
            _result.Firstname.Should().Be(_mockHttpResult.Firstname);
            _result.Lastname.Should().Be(_mockHttpResult.Lastname);
            _result.ProviderUkprn.Should().Be(_mockHttpResult.ProviderUkprn);
            _result.ProviderName.Should().Be(_mockHttpResult.ProviderName);
            _result.PathwayLarId.Should().Be(_mockHttpResult.PathwayLarId);
            _result.PathwayName.Should().Be(_mockHttpResult.PathwayName);
            _result.PathwayAssessmentSeries.Should().Be(_mockHttpResult.PathwayAssessmentSeries);
            _result.PathwayAssessmentId.Should().Be(_mockHttpResult.PathwayAssessmentId);
            _result.SpecialismLarId.Should().Be(_mockHttpResult.SpecialismLarId);
            _result.SpecialismName.Should().Be(_mockHttpResult.SpecialismName);
            _result.SpecialismAssessmentSeries.Should().Be(_mockHttpResult.SpecialismAssessmentSeries);
            _result.SpecialismAssessmentId.Should().Be(_mockHttpResult.SpecialismAssessmentId);
            _result.Status.Should().Be(_mockHttpResult.Status);
            _result.IsCoreEntryEligible.Should().BeTrue();
        }
    }
}
