using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddPathwayResultReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private readonly AdminAddPathwayResultViewModel _model = ViewModel;
        private readonly AdminAddPathwayResultReviewChangesViewModel _reviewChangesModel = new();

        public override void Given()
        {
            CacheService.GetAsync<AdminAddPathwayResultViewModel>(CacheKey).Returns(_model);
            AdminDashboardLoader.CreateAdminAddPathwayResultReviewChanges(_model).Returns(_reviewChangesModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddPathwayResultViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).CreateAdminAddPathwayResultReviewChanges(_model);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddPathwayResultReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_reviewChangesModel);
        }
    }
}