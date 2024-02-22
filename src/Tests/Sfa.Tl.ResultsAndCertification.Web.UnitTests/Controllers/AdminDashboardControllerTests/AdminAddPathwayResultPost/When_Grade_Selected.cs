using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddPathwayResultPost
{
    public class When_Grade_Selected : TestSetup
    {
        private AdminAddPathwayResultViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(selectedGradeCode: "PCG1");
        }

        public async override Task When()
        {
            Result = await Controller.AdminAddPathwayResultAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).LoadAdminAddPathwayResultGrades(_viewModel);
            CacheService.Received(1).SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.AdminAddPathwayResultReviewChanges);
        }
    }
}