using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSearchLearnerGet
{
    public class When_PopulateUln_IsTrue : TestSetup
    {
        private PrsSearchLearnerViewModel cacheSearchPrsViewModel;

        public override void Given() 
        {
            PopulateUln = true;
            cacheSearchPrsViewModel = new PrsSearchLearnerViewModel { SearchUln = "123465790" };
            
            CacheService.GetAsync<PrsSearchLearnerViewModel>(CacheKey)
                .Returns(cacheSearchPrsViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsSearchLearnerViewModel));

            var model = viewResult.Model as PrsSearchLearnerViewModel;
            model.Should().NotBeNull();
            model.SearchUln.Should().Be(cacheSearchPrsViewModel.SearchUln);

            // Breadcrumb
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.StartReviewsAndAppeals);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Reviews_And_Appeals);
        }
    }
}
