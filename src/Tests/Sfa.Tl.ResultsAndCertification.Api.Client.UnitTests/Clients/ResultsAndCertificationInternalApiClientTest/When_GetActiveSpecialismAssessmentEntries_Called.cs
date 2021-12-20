using FluentAssertions;
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
    public class When_GetActiveSpecialismAssessmentEntries_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        protected IEnumerable<AssessmentEntryDetails> _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private IEnumerable<AssessmentEntryDetails> _result;
        private string _specialismAssessmentIds;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<AssessmentEntryDetails>
            {
                new AssessmentEntryDetails
                {
                    ProfileId = 1,
                    AssessmentId = 11,
                    AssessmentSeriesId = 111,
                    AssessmentSeriesName = "Summer 2021"
                },

                 new AssessmentEntryDetails
                {
                    ProfileId = 1,
                    AssessmentId = 12,
                    AssessmentSeriesId = 121,
                    AssessmentSeriesName = "Summer 2021"
                }
            };

            _specialismAssessmentIds = "11|12";
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IEnumerable<AssessmentEntryDetails>>(_mockHttpResult, string.Format(ApiConstants.GetActiveSpecialismAssessmentEntriesUri, _ukprn, _specialismAssessmentIds), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetActiveSpecialismAssessmentEntriesAsync(_ukprn, _specialismAssessmentIds);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count().Should().Be(_mockHttpResult.Count());

            var expectedResult = _mockHttpResult.ToList();
            var actualResult = _result.ToList();
            for (int i = 0; i < _mockHttpResult.Count(); i++)
            {
                actualResult[i].ProfileId.Should().Be(expectedResult[i].ProfileId);
                actualResult[i].AssessmentId.Should().Be(expectedResult[i].AssessmentId);
                actualResult[i].AssessmentSeriesId.Should().Be(expectedResult[i].AssessmentSeriesId);
                actualResult[i].AssessmentSeriesName.Should().Be(expectedResult[i].AssessmentSeriesName);
            }
        }
    }
}
