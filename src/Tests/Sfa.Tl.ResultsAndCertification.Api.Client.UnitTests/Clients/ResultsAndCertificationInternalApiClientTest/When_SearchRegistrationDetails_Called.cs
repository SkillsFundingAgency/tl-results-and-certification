using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_SearchRegistrationDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private PagedResponse<SearchRegistrationDetail> _actualResult;
        private PagedResponse<SearchRegistrationDetail> _mockApiResponse;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;

        private readonly SearchRegistrationRequest _apiRequest = new()
        {
            AoUkprn = 10009696,
            SearchKey = "Johnson"
        };

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockApiResponse = new PagedResponse<SearchRegistrationDetail>
            {
                TotalRecords = 150,
                Records = new List<SearchRegistrationDetail>
                {
                    new()
                    {
                        Uln = 1234567890,
                        Firstname = "Jessica",
                        Lastname = "Johnson",
                        ProviderName = "Barnsley College",
                        ProviderUkprn = 10000536,
                        AcademicYear = 2021
                    }
                },
                PagerInfo = new Pager(1, 1, 10)
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PagedResponse<SearchRegistrationDetail>>(_mockApiResponse, ApiConstants.SearchRegistrationDetailsUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_apiRequest)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.SearchRegistrationDetailsAsync(_apiRequest);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_mockApiResponse);
        }
    }
}