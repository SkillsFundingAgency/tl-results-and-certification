﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetTlevelDetailsByPathwayId_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private readonly long ukprn = 1024;
        private readonly int tlevelId = 99;
        protected TlevelPathwayDetails _result;

        protected readonly string RouteName = "Construction";
        protected readonly string PathwayName = "Design";
        protected readonly string PathwayCode = "548515161";
        protected readonly string TlevelTitle = "Construction Design";
        protected List<SpecialismDetails> Specialisms;
        protected readonly int Status = 1;

        private ResultsAndCertificationInternalApiClient _apiClient;
        private TlevelPathwayDetails _mockHttpResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://xtz.com" }
            };

            Specialisms = new List<SpecialismDetails> {
                new SpecialismDetails { Name = "Civil Engineering", Code = "97865897" },
                new SpecialismDetails { Name = "Assisting teaching", Code = "7654321" }
            };

            _mockHttpResult = new TlevelPathwayDetails
            {
                TlevelTitle = TlevelTitle,
                PathwayName = PathwayName,
                PathwayCode = PathwayCode,
                RouteName = RouteName,
                Specialisms = Specialisms,
                PathwayStatusId = Status
            };
            HttpClient = new HttpClient(new MockHttpMessageHandler<TlevelPathwayDetails>(_mockHttpResult, string.Format(ApiConstants.TlevelDetailsUri, ukprn, tlevelId), HttpStatusCode.OK));
        }

        public override void Given()
        {            
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, tlevelId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.RouteName.Should().Be(RouteName);
            _result.PathwayName.Should().Be(PathwayName);
            _result.PathwayCode.Should().Be(PathwayCode);
            _result.TlevelTitle.Should().Be(TlevelTitle);
            _result.PathwayStatusId.Should().Be(Status);

            _result.Specialisms.Should().NotBeNullOrEmpty();
            _result.Specialisms.Count().Should().Be(2);
            _result.Specialisms.Should().BeEquivalentTo(Specialisms);
        }
    }
}
