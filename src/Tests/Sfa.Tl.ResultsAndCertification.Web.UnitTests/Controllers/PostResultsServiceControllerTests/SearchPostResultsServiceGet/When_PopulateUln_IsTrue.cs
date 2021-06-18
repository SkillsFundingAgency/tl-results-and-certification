using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.SearchPostResultsServiceGet
{
    public class When_PopulateUln_IsTrue : TestSetup
    {
        private SearchPostResultsServiceViewModel cacheSearchPrsViewModel;

        public override void Given() 
        {
            PopulateUln = true;
            cacheSearchPrsViewModel = new SearchPostResultsServiceViewModel { SearchUln = "123465790" };
            
            CacheService.GetAsync<SearchPostResultsServiceViewModel>(CacheKey)
                .Returns(cacheSearchPrsViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchPostResultsServiceViewModel));

            var model = viewResult.Model as SearchPostResultsServiceViewModel;
            model.Should().NotBeNull();
            model.SearchUln.Should().Be(cacheSearchPrsViewModel.SearchUln);

            // Breadcrumb
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.StartReviewsAndAppeals);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Reviews_And_Appeals);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
        }
    }
}
