using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ReportIssueAsync
{
    public class Then_ApiResponse_True_Is_Returned : When_Called_Method_ReportIssueAsync
    {
        [Fact]
        public void Then_ApiResponse_Is_True()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
