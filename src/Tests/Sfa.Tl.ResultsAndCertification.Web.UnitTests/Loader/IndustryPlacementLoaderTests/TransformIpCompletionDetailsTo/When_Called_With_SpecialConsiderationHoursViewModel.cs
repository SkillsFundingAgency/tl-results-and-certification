using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.TransformIpCompletionDetailsTo
{
    public class When_Called_With_SpecialConsiderationHoursViewModel : TestSetup
    {
        public SpecialConsiderationHoursViewModel ActualResult;

        public override void Given()
        {
            IpCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                LearnerName = "First Last",
            };
        }

        public override async Task When()
        {
            ActualResult = await Loader.TransformIpCompletionDetailsTo<SpecialConsiderationHoursViewModel>(IpCompletionViewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.ProfileId.Should().Be(IpCompletionViewModel.ProfileId);
            ActualResult.LearnerName.Should().Be(IpCompletionViewModel.LearnerName);
        }
    }
}
