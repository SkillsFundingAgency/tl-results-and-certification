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
    public class When_GetPathwaySpecialismsByPathwayLarIdAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private Task<PathwaySpecialisms> _result;
        private readonly long _ukprn = 12345678;
        private readonly string _pathwayLarId = "987654321";
        protected PathwaySpecialisms _mockHttpResult;
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

            _mockHttpResult = new PathwaySpecialisms
            {
                Id = 1,
                PathwayCode = "12345",
                PathwayName = "Test 1",
                Specialisms = new List<SpecialismDetails> { new SpecialismDetails { Id = 1, Code = "76543", Name = "Specialism 1" } }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PathwaySpecialisms>(_mockHttpResult, string.Format(ApiConstants.GetPathwaySpecialismsByPathwayLarIdAsyncUri, _ukprn, _pathwayLarId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetPathwaySpecialismsByPathwayLarIdAsync(_ukprn, _pathwayLarId);
        }

        [Fact]
        public void Then_Expected_No_Of_Specialism_Details_Are_Returned()
        {
            _result.Result.Should().NotBeNull();
            _result.Result.Specialisms.Count().Should().Be(_mockHttpResult.Specialisms.Count);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();

            actualResult.Id.Should().Be(_mockHttpResult.Id);
            actualResult.PathwayName.Should().Be(_mockHttpResult.PathwayName);
            actualResult.PathwayCode.Should().Be(_mockHttpResult.PathwayCode);

            var expectedSpecialismResult = _mockHttpResult.Specialisms.FirstOrDefault();
            var actualSpecialismResult = actualResult.Specialisms.FirstOrDefault();
            actualSpecialismResult.Should().NotBeNull();

            actualSpecialismResult.Id.Should().Be(expectedSpecialismResult.Id);
            actualSpecialismResult.Name.Should().Be(expectedSpecialismResult.Name);
            actualSpecialismResult.Code.Should().Be(expectedSpecialismResult.Code);
        }
    }
}
