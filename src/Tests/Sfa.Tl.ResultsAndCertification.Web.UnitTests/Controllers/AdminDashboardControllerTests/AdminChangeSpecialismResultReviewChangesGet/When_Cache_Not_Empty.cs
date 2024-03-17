using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangeSpecialismResultReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private readonly AdminChangeSpecialismResultViewModel _model = ViewModel;
        private readonly AdminChangeSpecialismResultReviewChangesViewModel _reviewChangesModel = new();

        public override void Given()
        {
            CacheService.GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey).Returns(_model);
            AdminDashboardLoader.CreateAdminChangeSpecialismResultReviewChanges(_model).Returns(_reviewChangesModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).CreateAdminChangeSpecialismResultReviewChanges(_model);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminChangeSpecialismResultReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_reviewChangesModel);
        }
    }
}