using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayAppealPost
{
    public class When_No_Selected : TestSetup
    {
        private AdminOpenPathwayAppealViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(doYouWantToOpenAppeal: false);
        }

        public async override Task When()
        {
            Result = await Controller.AdminOpenPathwayAppealAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToRouteResult(nameof(RouteConstants.AdminLearnerRecord), ("pathwayId", _viewModel.RegistrationPathwayId));
        }
    }
}