using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetSearchLearnerFilters_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // inputs
        private readonly long _providerUkprn = 12345678;
        
        // results
        private SearchLearnerFilters _actualResult;
        private SearchLearnerFilters _mockApiResponse;

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

            _mockApiResponse = new SearchLearnerFilters
            {
                AcademicYears = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 1, Name = "2020 to 2021", IsSelected = false },
                    new FilterLookupData { Id = 2, Name = "2021 to 2022", IsSelected = false }
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<SearchLearnerFilters>(_mockApiResponse, string.Format(ApiConstants.SearchLearnerFiltersUri, _providerUkprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetSearchLearnerFiltersAsync(_providerUkprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();

            // Assert AcademicYears
            _actualResult.AcademicYears.Should().HaveCount(_mockApiResponse.AcademicYears.Count);
            _actualResult.AcademicYears.Should().BeEquivalentTo(_mockApiResponse.AcademicYears);
        }
    }
}
