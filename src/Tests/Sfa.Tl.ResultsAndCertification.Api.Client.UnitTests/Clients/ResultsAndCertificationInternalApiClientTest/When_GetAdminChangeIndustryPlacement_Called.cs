using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAdminChangeIndustryPlacement_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        private readonly int _pathwayId = 1;
        protected AdminLearnerRecord _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private AdminLearnerRecord _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new AdminLearnerRecord
            {
                PathwayId = 1,
                Uln = 1234567890,
                Name = "John Smith",
                DateofBirth = System.DateTime.UtcNow.AddYears(-29),
                ProviderName = "NCFE",
                RegistrationPathwayId = 1,
                TlPathwayId = 1,
                ProviderUkprn = 1008900,
                TlevelName = "",
                AcademicYear = 2020,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed

            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AdminLearnerRecord>(_mockHttpResult, string.Format(ApiConstants.GetAdminLearnerRecordUri, _pathwayId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAdminLearnerRecordAsync(_pathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Uln.Should().Be(_mockHttpResult.Uln);
            _result.Name.Should().Be(_mockHttpResult.Name);
            _result.TlevelName.Should().Be(_mockHttpResult.TlevelName);
            _result.DateofBirth.Should().Be(_mockHttpResult.DateofBirth);
            _result.AcademicYear.Should().Be(_mockHttpResult.AcademicYear);
            _result.IndustryPlacementStatus.Should().Be(_mockHttpResult.IndustryPlacementStatus);
        }
    }
}
