using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetResultDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        private readonly int _profileId = 1;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Active;
        protected ResultDetails _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private ResultDetails _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new ResultDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Now.AddYears(-30),
                TlevelTitle = "Tlevel title",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                PathwayName = "Pathway",
                PathwayLarId = "7654321",
                PathwayAssessmentSeries = "Summer 2021",
                AppealEndDate = DateTime.Today.AddDays(7),
                PathwayAssessmentId = 11,
                PathwayResultId = 123,
                PathwayResult = "A",
                PathwayPrsStatus = PrsStatus.BeingAppealed,
                Status = RegistrationPathwayStatus.Active
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<ResultDetails>(_mockHttpResult, string.Format(ApiConstants.GetResultDetailsUri, _ukprn, _profileId, (int)_registrationPathwayStatus), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetResultDetailsAsync(_ukprn, _profileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.ProfileId.Should().Be(_mockHttpResult.ProfileId);
            _result.Uln.Should().Be(_mockHttpResult.Uln);
            _result.Firstname.Should().Be(_mockHttpResult.Firstname);
            _result.Lastname.Should().Be(_mockHttpResult.Lastname);
            _result.DateofBirth.Should().Be(_mockHttpResult.DateofBirth);
            _result.ProviderUkprn.Should().Be(_mockHttpResult.ProviderUkprn);
            _result.ProviderName.Should().Be(_mockHttpResult.ProviderName);
            _result.PathwayName.Should().Be(_mockHttpResult.PathwayName);
            _result.PathwayLarId.Should().Be(_mockHttpResult.PathwayLarId);
            _result.PathwayAssessmentSeries.Should().Be(_mockHttpResult.PathwayAssessmentSeries);
            _result.AppealEndDate.Should().Be(_mockHttpResult.AppealEndDate);
            _result.PathwayAssessmentId.Should().Be(_mockHttpResult.PathwayAssessmentId);
            _result.PathwayResultId.Should().Be(_mockHttpResult.PathwayResultId);
            _result.PathwayResult.Should().Be(_mockHttpResult.PathwayResult);
            _result.PathwayPrsStatus.Should().Be(_mockHttpResult.PathwayPrsStatus);
            _result.Status.Should().Be(_mockHttpResult.Status);
        }
    }
}
