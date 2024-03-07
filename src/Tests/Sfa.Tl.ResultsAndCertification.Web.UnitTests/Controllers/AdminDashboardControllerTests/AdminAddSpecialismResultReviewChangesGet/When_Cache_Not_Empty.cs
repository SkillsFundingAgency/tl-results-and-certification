using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddSpecialismResultReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private readonly AdminAddSpecialismResultViewModel _model = ViewModel;
        private readonly AdminAddSpecialismResultReviewChangesViewModel _reviewChangesModel = new();

        public override void Given()
        {
            CacheService.GetAsync<AdminAddSpecialismResultViewModel>(CacheKey).Returns(_model);
            AdminDashboardLoader.CreateAdminAddSpecialismResultReviewChanges(_model).Returns(_reviewChangesModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddSpecialismResultViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).CreateAdminAddSpecialismResultReviewChanges(_model);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddSpecialismResultReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_reviewChangesModel);
        }
    }
}