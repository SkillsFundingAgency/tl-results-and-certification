using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using NSubstitute;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetIpLookupData_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        
        private readonly IpLookupType _ipLookupType = IpLookupType.IndustryPlacementModel;
        private readonly int _pathwayId = 1;
        
        private ResultsAndCertificationInternalApiClient _apiClient;
        private IList<IpLookupData> _mockHttpResult;
        private IList<IpLookupData> _actualResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<IpLookupData>
            {
                new IpLookupData { Id = 1, Name = "Medical Condition", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = null, ShowOption = null },
                new IpLookupData { Id = 2, Name = "Placement Withdraw", StartDate = DateTime.UtcNow, EndDate = null, ShowOption = null },
                new IpLookupData { Id = 3, Name = "Bereavement", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = DateTime.UtcNow.AddYears(2), ShowOption = null },
                new IpLookupData { Id = 4, Name = "Crisis", StartDate = DateTime.UtcNow.AddYears(1), EndDate = null, ShowOption = null },
                new IpLookupData { Id = 5, Name = "Circumstancs", StartDate = DateTime.UtcNow.AddYears(1), EndDate = DateTime.UtcNow.AddYears(4), ShowOption = true },
                new IpLookupData { Id = 6, Name = "Placement Withdraw", StartDate = DateTime.UtcNow.AddYears(1), EndDate = DateTime.UtcNow.AddYears(4), ShowOption = false },
            };
        }
        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IList<IpLookupData>>(_mockHttpResult, string.Format(ApiConstants.GetIpLookupDataUri, (int)_ipLookupType, _pathwayId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetIpLookupDataAsync(_ipLookupType, _pathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNullOrEmpty();
            _actualResult.Should().HaveCount(_mockHttpResult.Count());
            _actualResult.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}
