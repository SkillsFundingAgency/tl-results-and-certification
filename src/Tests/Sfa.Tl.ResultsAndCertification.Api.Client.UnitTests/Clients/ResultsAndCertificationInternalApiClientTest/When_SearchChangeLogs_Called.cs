using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_SearchChangeLogs_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private PagedResponse<AdminSearchChangeLog> _actualResult;
        private PagedResponse<AdminSearchChangeLog> _mockApiResponse;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;

        private readonly AdminSearchChangeLogRequest _apiRequest = new() { SearchKey = "Johnson" };

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockApiResponse = new PagedResponse<AdminSearchChangeLog>
            {
                TotalRecords = 150,
                Records = new List<AdminSearchChangeLog>
                {
                    new AdminSearchChangeLog
                    {
                        ChangeLogId = 1,
                        DateAndTimeOfChange = new DateTime(2024, 1, 1),
                        ZendeskTicketID = "1234567-AB",
                        LearnerFirstname = "Jessica",
                        LearnerLastname = "Johnson",
                        Uln = 1234567890,
                        ProviderName = "Barnsley College",
                        ProviderUkprn = 10000536,
                        LastUpdatedBy = "admin-user-01"
                    }
                },
                PagerInfo = new Pager(1, 1, 10)
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PagedResponse<AdminSearchChangeLog>>(_mockApiResponse, ApiConstants.SearchChangeLogs, HttpStatusCode.OK, JsonConvert.SerializeObject(_apiRequest)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.SearchChangeLogsAsync(_apiRequest);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_mockApiResponse);
        }
    }
}