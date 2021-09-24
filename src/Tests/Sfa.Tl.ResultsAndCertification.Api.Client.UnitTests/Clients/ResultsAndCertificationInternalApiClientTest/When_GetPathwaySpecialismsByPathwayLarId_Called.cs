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
    public class When_GetPathwaySpecialismsByPathwayLarId_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private PathwaySpecialisms _result;
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
                Specialisms = new List<PathwaySpecialismCombination>
                {
                    new PathwaySpecialismCombination
                    {
                        SpecialismDetails = new List<SpecialismDetails>
                        {
                            new SpecialismDetails { Id = 1, Code = "11111111", Name = "Design" },
                            new SpecialismDetails { Id = 2, Code = "22222222", Name = "Engineering" }
                        }
                    },
                    new PathwaySpecialismCombination
                    {
                        SpecialismDetails = new List<SpecialismDetails>
                        {
                            new SpecialismDetails { Id = 3, Code = "33333333", Name = "Arts" },
                        }
                    }
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PathwaySpecialisms>(_mockHttpResult, string.Format(ApiConstants.GetPathwaySpecialismsByPathwayLarIdAsyncUri, _ukprn, _pathwayLarId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetPathwaySpecialismsByPathwayLarIdAsync(_ukprn, _pathwayLarId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_mockHttpResult.Id);
            _result.PathwayName.Should().Be(_mockHttpResult.PathwayName);
            _result.PathwayCode.Should().Be(_mockHttpResult.PathwayCode);
            _result.Specialisms.Count().Should().Be(_mockHttpResult.Specialisms.Count());

            var expectedSpecialismCombination = _mockHttpResult.Specialisms.FirstOrDefault();
            var actualSpecialismCombination = _result.Specialisms.FirstOrDefault();
            actualSpecialismCombination.SpecialismDetails.Count().Should().Be(actualSpecialismCombination.SpecialismDetails.Count());

            var expectedSpecialism = expectedSpecialismCombination.SpecialismDetails.FirstOrDefault();
            var actualSpecialism = actualSpecialismCombination.SpecialismDetails.FirstOrDefault();
            actualSpecialism.Should().NotBeNull();

            actualSpecialism.Id.Should().Be(expectedSpecialism.Id);
            actualSpecialism.Name.Should().Be(expectedSpecialism.Name);
            actualSpecialism.Code.Should().Be(expectedSpecialism.Code);
        }
    }
}
