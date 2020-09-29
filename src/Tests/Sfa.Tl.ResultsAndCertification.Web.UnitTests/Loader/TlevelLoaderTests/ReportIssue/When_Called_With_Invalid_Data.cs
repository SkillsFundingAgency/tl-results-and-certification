using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ReportIssue
{
    public class When_Called_With_Invalid_Data : TestSetup
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
        public void Then_Expected_Methods_Called()
        {
            Mapper.Received().Map<VerifyTlevelDetails>(TlevelQueryViewModel);
            InternalApiClient.Received().VerifyTlevelAsync(VerifyTlevelDetails);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
