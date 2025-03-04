using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAllAwardingOrganisations_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly IEnumerable<AwardingOrganisationMetadata> _mockHttpResult = new[]
        {
            new AwardingOrganisationMetadata
            {
                Id = 1,
                Ukprn = 10009696,
                DisplayName = "NCFE"
            },
            new AwardingOrganisationMetadata
            {
                Id = 2,
                Ukprn = 10022490,
                DisplayName = "Pearson"
            }
        };

        private ResultsAndCertificationInternalApiClient _apiClient;
        private IEnumerable<AwardingOrganisationMetadata> _result;

        public override void Setup()
        {
        }

        public override void Given()
        {
            ITokenServiceClient tokenServiceClient = Substitute.For<ITokenServiceClient>();

            ResultsAndCertificationConfiguration configuration = new()
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            HttpClient = new HttpClient(new MockHttpMessageHandler<IEnumerable<AwardingOrganisationMetadata>>(_mockHttpResult, ApiConstants.GetAllAwardingOrganisations, HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, tokenServiceClient, configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAllAwardingOrganisationsAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}