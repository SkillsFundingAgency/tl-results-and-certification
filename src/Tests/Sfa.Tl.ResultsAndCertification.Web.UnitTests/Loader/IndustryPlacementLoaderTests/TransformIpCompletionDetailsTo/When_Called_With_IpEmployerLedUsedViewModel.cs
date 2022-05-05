using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.TransformIpCompletionDetailsTo
{
    public class When_Called_With_IpEmployerLedUsedViewModel : TestSetup
    {
        public IpEmployerLedUsedViewModel ActualResult;

        public override void Given()
        {
            IpCompletionViewModel = new IpCompletionViewModel
            {
                LearnerName = "First Last",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };
        }

        public override async Task When()
        {
            ActualResult = await Loader.TransformIpCompletionDetailsTo<IpEmployerLedUsedViewModel>(IpCompletionViewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.LearnerName.Should().Be(IpCompletionViewModel.LearnerName);
        }
    }
}
