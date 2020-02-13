using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.GetAllTlevelsByAwardingOrganisationAsync;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetAllTlevelsByAwardingOrganisationAsync
{
    public class Then_HttpStatusCode_200_Returned : When_GetAllTlevelsByAwardingOrganisationAsync_Is_Called
    {
        [Fact]
        public void Then_Expected_Result_Returned()
        {
            var expectedResult = Result.Result.FirstOrDefault();

            expectedResult.RouteName.Should().Be(RouteName);
            expectedResult.PathwayName.Should().Be(PathwayName);
            expectedResult.StatusId.Should().Be(1);
        }
    }
}
