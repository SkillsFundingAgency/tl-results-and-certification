using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetLookupData_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private LookupCategory _pathwayComponentGrade;
        protected IList<LookupData> _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private IList<LookupData> _result;

        public override void Setup()
        {
            _pathwayComponentGrade = LookupCategory.PathwayComponentGrade;
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<LookupData>
            {
                new LookupData { Id = 1, Code = "C1", Value = "V1" },
                new LookupData { Id = 2, Code = "C2", Value = "V2" },
                new LookupData { Id = 3, Code = "C3", Value = "V3" },
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IList<LookupData>>(_mockHttpResult, string.Format(ApiConstants.GetLookupDataUri, (int)_pathwayComponentGrade), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetLookupDataAsync(_pathwayComponentGrade);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count().Should().Be(_mockHttpResult.Count);

            for (int i = 0; i < _result.Count; i++)
            {
                _result[i].Id.Should().Be(_mockHttpResult[i].Id);
                _result[i].Code.Should().Be(_mockHttpResult[i].Code);
                _result[i].Value.Should().Be(_mockHttpResult[i++].Value);
            }
        }
    }
}
