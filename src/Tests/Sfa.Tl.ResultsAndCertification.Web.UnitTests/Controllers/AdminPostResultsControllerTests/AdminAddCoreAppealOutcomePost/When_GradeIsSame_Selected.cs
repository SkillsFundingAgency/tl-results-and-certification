using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreAppealOutcomePost
{
    public class When_GradeIsSame_Selected : TestSetup
    {
        private AdminAddCoreAppealOutcomeViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(whatIsAppealOutcome: false);
        }

        public async override Task When()
        {
            Result = await Controller.AdminAddCoreAppealOutcomeAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToRouteResult(nameof(RouteConstants.AdminReviewChangesAppealOutcomeCore), (("isSameGrade", true)));
        }
    }
}