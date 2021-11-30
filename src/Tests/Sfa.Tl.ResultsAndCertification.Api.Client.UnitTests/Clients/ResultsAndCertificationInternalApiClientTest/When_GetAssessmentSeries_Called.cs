using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAssessmentSeries_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected IList<AssessmentSeriesDetails> _mockHttpResult;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private IList<AssessmentSeriesDetails> _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<AssessmentSeriesDetails>
            {
                new AssessmentSeriesDetails { Id = 1, ComponentType = ComponentType.Core, Name = "Summer 2021", Description = "Summer 2021", Year = 2021, StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(10), AppealEndDate = DateTime.UtcNow.AddDays(15) },
                new AssessmentSeriesDetails { Id = 1, ComponentType = ComponentType.Specialism, Name = "Summer 2022", Description = "Summer 2022", Year = 2022, StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(10), AppealEndDate = DateTime.UtcNow.AddDays(15) }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IList<AssessmentSeriesDetails>>(_mockHttpResult, ApiConstants.GetAssessmentSeriesDetailsUri, HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAssessmentSeriesAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count().Should().Be(_mockHttpResult.Count);

            for (int i = 0; i < _result.Count; i++)
            {
                _result[i].Id.Should().Be(_mockHttpResult[i].Id);
                _result[i].ComponentType.Should().Be(_mockHttpResult[i].ComponentType);
                _result[i].Name.Should().Be(_mockHttpResult[i].Name);
                _result[i].Description.Should().Be(_mockHttpResult[i].Description);
                _result[i].Year.Should().Be(_mockHttpResult[i].Year);
                _result[i].StartDate.Should().Be(_mockHttpResult[i].StartDate);
                _result[i].EndDate.Should().Be(_mockHttpResult[i].EndDate);
                _result[i].AppealEndDate.Should().Be(_mockHttpResult[i].AppealEndDate);
            }
        }
    }
}
