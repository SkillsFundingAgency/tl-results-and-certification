using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using Xunit;
using NSubstitute;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordGet
{
    public class When_Cache_IsFound : TestSetup
    {
        private SearchLearnerRecordViewModel _cacheModel;

        public override void Given()
        {
            _cacheModel = new SearchLearnerRecordViewModel { SearchUln = "1234567890" };
            CacheService.GetAndRemoveAsync<SearchLearnerRecordViewModel>(CacheKey).Returns(_cacheModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as SearchLearnerRecordViewModel;
            model.SearchUln.Should().Be(_cacheModel.SearchUln);
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Manage_Learner_Records);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ManageLearnerRecordsDashboard);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNull();
        }
    }
}
