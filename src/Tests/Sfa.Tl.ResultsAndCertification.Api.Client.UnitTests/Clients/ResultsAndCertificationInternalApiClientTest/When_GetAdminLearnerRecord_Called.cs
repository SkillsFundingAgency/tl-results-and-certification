using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static System.Net.WebRequestMethods;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAdminLearnerRecordAsync_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly int _profileId = 5369613;
        private AdminLearnerRecord _actualResult;
        private AdminLearnerRecord _mockApiResponse;

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

            _mockApiResponse = new AdminLearnerRecord
            {
                FirstName = "John",
                LastName = "Smith",
                Uln = 1234567890,
                Provider = "Barnsley College",
                AcademicYear = 2022,
                TLevel = "Building Services Engineering",
                DisplayAcademicYear = "2022 to 2023",
                TLevelStartYear = 2021,
                Ukprn = 1234567890
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AdminLearnerRecord>(_mockApiResponse, string.Format(ApiConstants.GetAdminLearnerRecordUri,_profileId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetAdminLearnerRecordAsync(_profileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_mockApiResponse);
        }
    }
}
