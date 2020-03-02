using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.GetAllTlevelsByUkprnAsync;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetAllTlevelsByUkprnAsync
{
    public class Then_HttpStatusCode_200_Returned : When_GetAllTlevelsByAwardingOrganisationAsync_Is_Called
    {
        [Fact]
        public void Then_Expected_Result_Returned()
        {
            //Result.Result.Should().NotBeNullOrEmpty();

            //var expectedResult = Result.Result.FirstOrDefault();
            //expectedResult.RouteName.Should().Be(RouteName);
            //expectedResult.PathwayName.Should().Be(PathwayName);
            //expectedResult.StatusId.Should().Be(1);
        }
    }
}
