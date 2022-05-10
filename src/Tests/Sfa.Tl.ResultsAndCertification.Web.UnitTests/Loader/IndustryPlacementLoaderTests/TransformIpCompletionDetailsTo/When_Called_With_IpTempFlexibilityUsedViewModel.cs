using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.TransformIpCompletionDetailsTo
{
    public class When_Called_With_IpTempFlexibilityUsedViewModel : TestSetup
    {
        public IpTempFlexibilityUsedViewModel ActualResult;

        public override void Given()
        {
            IpCompletionViewModel = new IpCompletionViewModel
            {
                LearnerName = "First Last",
            };
        }

        public override async Task When()
        {
            ActualResult = await Loader.TransformIpCompletionDetailsTo<IpTempFlexibilityUsedViewModel>(IpCompletionViewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.LearnerName.Should().Be(IpCompletionViewModel.LearnerName);
        }
    }
}
