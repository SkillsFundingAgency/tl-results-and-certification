using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAdminSearchLearnerFilters_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private AdminSearchLearnerFilters _actualResult;
        private AdminSearchLearnerFilters _mockApiResponse;

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

            _mockApiResponse = new AdminSearchLearnerFilters
            {
                AwardingOrganisations = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 1, Name = "Ncfe", IsSelected = false },
                    new FilterLookupData { Id = 2, Name = "Pearson", IsSelected = false }
                },
                AcademicYears = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 1, Name = "2020 to 2021", IsSelected = false }
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AdminSearchLearnerFilters>(_mockApiResponse, ApiConstants.GetAdminSearchLearnerFiltersUri, HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetAdminSearchLearnerFiltersAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_mockApiResponse);
        }
    }
}