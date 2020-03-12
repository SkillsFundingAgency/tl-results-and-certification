using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ConfirmTlevelAsync
{
    public class Then_ApiResponse_True_Is_Returned : When_Called_Method_ConfirmTlevelAsync
    {
        [Fact]
        public void Then_ApiResponse_Is_True()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
