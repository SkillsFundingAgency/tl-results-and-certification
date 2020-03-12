using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ConfirmTlevelAsync
{
    public class Then_ApiResponse_False_Is_Returned : When_Called_Method_ConfirmTlevelAsync
    {
        public override void Given()
        {
            PathwayId = 99;
            ExpectedResult = false;

            ConfirmTlevelViewModel = new ConfirmTlevelViewModel { PathwayId = PathwayId, TqAwardingOrganisationId = PathwayId };
            VerifyTlevelDetails = new VerifyTlevelDetails { Id = PathwayId, TqAwardingOrganisationId = PathwayId, PathwayStatusId = StatusId };

            Mapper.Map<VerifyTlevelDetails>(ConfirmTlevelViewModel).Returns(VerifyTlevelDetails);
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
