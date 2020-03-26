using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.AddProviderTlevelsAsync
{
    public class Then_ApiResponse_True_Is_Returned : When_Called_Method_AddProviderTlevelsAsync
    {
        [Fact]
        public void Then_ApiResponse_Is_True()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
