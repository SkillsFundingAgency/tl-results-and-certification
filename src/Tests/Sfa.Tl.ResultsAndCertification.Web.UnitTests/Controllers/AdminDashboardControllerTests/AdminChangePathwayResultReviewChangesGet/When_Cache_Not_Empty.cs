using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangePathwayResultReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private readonly AdminChangePathwayResultViewModel _model = ViewModel;
        private readonly AdminChangePathwayResultReviewChangesViewModel _reviewChangesModel = new();

        public override void Given()
        {
            CacheService.GetAsync<AdminChangePathwayResultViewModel>(CacheKey).Returns(_model);
            AdminDashboardLoader.CreateAdminChangePathwayResultReviewChanges(_model).Returns(_reviewChangesModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangePathwayResultViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).CreateAdminChangePathwayResultReviewChanges(_model);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminChangePathwayResultReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_reviewChangesModel);
        }
    }
}