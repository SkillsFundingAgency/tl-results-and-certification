using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreRommOutcomePost
{
    public class When_GradeIsSame_Selected : TestSetup
    {
        private AdminAddCoreRommOutcomeViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(whatIsRommOutcome: false);
        }

        public async override Task When()
        {
            Result = await Controller.AdminAddCoreRommOutcomeAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToRouteResult(nameof(RouteConstants.AdminLearnerRecord), (("pathwayId", _viewModel.RegistrationPathwayId)));
        }
    }
}