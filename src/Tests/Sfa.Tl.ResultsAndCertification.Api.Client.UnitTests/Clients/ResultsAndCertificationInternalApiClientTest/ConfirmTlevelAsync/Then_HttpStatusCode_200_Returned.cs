using System.Linq;
using Xunit;
using FluentAssertions;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.ConfirmTlevelAsync
{
    public class Then_HttpStatusCode_200_Returned : When_ConfirmTlevelAsync_Is_Called
    {
        [Fact]
        public void Then_Expected_Result_Returned()
        {
            Result.Result.Should().BeTrue();
        }
    }
}
