using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ReportIssueAsync
{
    public class Then_ApiResponse_False_Is_Returned : When_Called_Method_ReportIssueAsync
    {
        public override void Given()
        {
            PathwayId = 99;
            ExpectedResult = false;

            TlevelQueryViewModel = new TlevelQueryViewModel { PathwayId = PathwayId, TqAwardingOrganisationId = PathwayId };
            VerifyTlevelDetails = new VerifyTlevelDetails { Id = PathwayId, TqAwardingOrganisationId = PathwayId, PathwayStatusId = StatusId };

            Mapper.Map<VerifyTlevelDetails>(TlevelQueryViewModel).Returns(VerifyTlevelDetails);
            InternalApiClient.VerifyTlevelAsync(VerifyTlevelDetails).Returns(ExpectedResult);

            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_ApiResponse_Is_False()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
