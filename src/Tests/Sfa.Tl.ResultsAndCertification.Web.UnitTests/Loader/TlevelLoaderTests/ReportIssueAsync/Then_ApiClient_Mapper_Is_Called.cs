using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ReportIssueAsync
{
    public class Then_ApiClient_Mapper_Is_Called : When_Called_Method_ReportIssueAsync
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().VerifyTlevelAsync(VerifyTlevelDetails);
        }

        [Fact]
        public void Then_Mapper_Is_Called()
        {
            Mapper.Received().Map<VerifyTlevelDetails>(TlevelQueryViewModel);
        }
    }
}
