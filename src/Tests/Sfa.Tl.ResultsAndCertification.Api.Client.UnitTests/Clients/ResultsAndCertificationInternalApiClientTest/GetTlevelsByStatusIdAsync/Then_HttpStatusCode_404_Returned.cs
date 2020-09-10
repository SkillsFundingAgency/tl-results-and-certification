using System.Net;
using System.Net.Http;
using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetTlevelsByStatusIdAsync
{
    public class Then_HttpStatusCode_404_Returned : When_GetTlevelsByStatusId_Called
    {
        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IEnumerable<AwardingOrganisationPathwayStatus>>(null, null, HttpStatusCode.NotFound));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.IsCompletedSuccessfully.Should().BeFalse();
            Result.IsFaulted.Should().BeTrue();
            Result.Exception.Should().NotBeNull();
            Result.Exception.Message.Should().NotBeNullOrEmpty();
            Result.Exception.Message.Should().Contain(((int)HttpStatusCode.NotFound).ToString());
        }
    }
}
