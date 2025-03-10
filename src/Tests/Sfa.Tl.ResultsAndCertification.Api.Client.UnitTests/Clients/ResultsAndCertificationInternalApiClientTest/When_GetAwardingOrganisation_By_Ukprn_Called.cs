using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAwardingOrganisation_By_Ukprn_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private const long Ukprn = 10009696;

        private readonly AwardingOrganisationMetadata _mockHttpResult = new()
        {
            Id = 1,
            Ukprn = Ukprn,
            DisplayName = "NCFE"
        };

        private ResultsAndCertificationInternalApiClient _apiClient;
        private AwardingOrganisationMetadata _result;

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

            HttpClient = new HttpClient(new MockHttpMessageHandler<AwardingOrganisationMetadata>(_mockHttpResult, string.Format(ApiConstants.GetAwardingOrganisationByUkprn, Ukprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, tokenServiceClient, configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAwardingOrganisationByUkprnAsync(Ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}