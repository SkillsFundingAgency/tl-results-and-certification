using System.Net;
using System.Linq;
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
    public class When_GetTlevelDetailsByPathwayId_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private readonly long ukprn = 1024;
        private readonly int tlevelId = 99;
        protected Task<TlevelPathwayDetails> Result;

        protected readonly string RouteName = "Construction";
        protected readonly string PathwayName = "Design";
        protected readonly List<string> Specialisms = new List<string> { "Civil Engineering", "Assisting teaching" };
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

            _mockHttpResult = new TlevelPathwayDetails
            {
                PathwayName = PathwayName,
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

        public override void When()
        {
            Result = _apiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, tlevelId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var expectedResult = Result.Result;

            expectedResult.Should().NotBeNull();
            expectedResult.RouteName.Should().Be(RouteName);
            expectedResult.PathwayName.Should().Be(PathwayName);
            expectedResult.PathwayStatusId.Should().Be(Status);

            expectedResult.Specialisms.Should().NotBeNullOrEmpty();
            expectedResult.Specialisms.Count().Should().Be(2);
            expectedResult.Specialisms.First().Should().Be(Specialisms.First());
        }
    }
}
