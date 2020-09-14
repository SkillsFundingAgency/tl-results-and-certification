using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetTlevelsByStatusIdAsync
{
    public class Then_HttpStatusCode_200_Returned : When_GetTlevelsByStatusId_Called
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNullOrEmpty();

            var expectedResult = Result.FirstOrDefault();
            expectedResult.TlevelTitle.Should().Be(TlevelTitle);
            expectedResult.StatusId.Should().Be(StatusId);
        }
    }
}
