using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetTelevelDetailsByPathwayIdAsyncTests
{
    public class Then_HttpStatusCode_200_Returned : When_GetTlevelDetailsByPathwayIdAsync_Is_Called
    {
        [Fact]
        public void Then_Expected_Result_Returned()
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
