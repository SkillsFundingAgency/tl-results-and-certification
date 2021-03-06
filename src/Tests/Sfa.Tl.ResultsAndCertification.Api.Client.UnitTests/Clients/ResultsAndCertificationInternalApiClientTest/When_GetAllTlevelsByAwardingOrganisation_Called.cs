﻿using System.Linq;
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

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAllTlevelsByAwardingOrganisation_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private readonly long ukprn = 1024;

        protected IEnumerable<AwardingOrganisationPathwayStatus> Result;
        protected readonly string TlevelTitle = "Construction";
        protected readonly int Status = 1;

        private ResultsAndCertificationInternalApiClient _apiClient;
        private IEnumerable<AwardingOrganisationPathwayStatus> _mockHttpResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "https://test.xyz.com" }
            };

            _mockHttpResult = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { TlevelTitle = TlevelTitle, StatusId = Status }
            };

            HttpClient = new HttpClient(
                new MockHttpMessageHandler<IEnumerable<AwardingOrganisationPathwayStatus>>(
                    _mockHttpResult, string.Format(ApiConstants.GetAllTLevelsUri, ukprn), HttpStatusCode.OK));
        }
        public override void Given()
        {            
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            Result = await _apiClient.GetAllTlevelsByUkprnAsync(ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNullOrEmpty();

            var expectedResult = Result.FirstOrDefault();
            expectedResult.TlevelTitle.Should().Be(TlevelTitle);
            expectedResult.StatusId.Should().Be(1);
        }
    }
}
