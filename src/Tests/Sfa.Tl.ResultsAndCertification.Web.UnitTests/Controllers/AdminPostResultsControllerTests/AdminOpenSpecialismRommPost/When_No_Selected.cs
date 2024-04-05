using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismRommPost
{
    public class When_No_Selected : TestSetup
    {
        private AdminOpenSpecialismRommViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(doYouWantToOpenRomm: false);
        }

        public async override Task When()
        {
            Result = await Controller.AdminOpenSpecialismRommAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToRouteResult(nameof(RouteConstants.AdminLearnerRecord), ("pathwayId", _viewModel.RegistrationPathwayId));
        }
    }
}