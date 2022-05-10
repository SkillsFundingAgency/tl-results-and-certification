using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessIndustryPlacementDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private bool _expectedApiResult;

        public override void Given()
        {
            ProviderUkprn = 987654321;

            ViewModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel
                {
                    ProfileId = 1,
                    RegistrationPathwayId = 1,
                    PathwayId = 7,
                    IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
                }
            };

            _expectedApiResult = false;

            InternalApiClient.ProcessIndustryPlacementDetailsAsync(Arg.Any<IndustryPlacementRequest>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
