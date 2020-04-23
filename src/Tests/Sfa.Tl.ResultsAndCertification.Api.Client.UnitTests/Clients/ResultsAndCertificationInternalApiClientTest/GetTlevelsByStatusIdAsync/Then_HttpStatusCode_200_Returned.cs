using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetTlevelsByStatusIdAsync
{
    public class Then_HttpStatusCode_200_Returned : When_GetTlevelsByStatusIdAsync_Is_Called
    {
        [Fact]
        public void Then_Expected_Result_Returned()
        {
            Result.Result.Should().NotBeNullOrEmpty();

            var expectedResult = Result.Result.FirstOrDefault();
            expectedResult.TlevelTitle.Should().Be(TlevelTitle);
            expectedResult.StatusId.Should().Be(StatusId);
        }
    }
}
