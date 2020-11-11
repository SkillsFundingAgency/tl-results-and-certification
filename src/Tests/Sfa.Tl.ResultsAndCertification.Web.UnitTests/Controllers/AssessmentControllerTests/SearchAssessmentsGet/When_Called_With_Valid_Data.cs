using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SearchAssessmentsGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private string _searchUln = "9895641231";
        
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<string>(Arg.Any<string>()).Returns(_searchUln);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchAssessmentsViewModel));

            var model = viewResult.Model as SearchAssessmentsViewModel;
            model.Should().NotBeNull();
            model.SearchUln.Should().Be(_searchUln);
        }

        [Fact]
        public void Then_Expected_Breadcrumb_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchAssessmentsViewModel));

            var model = viewResult.Model as SearchAssessmentsViewModel;
            model.Should().NotBeNull();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.AssessmentDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Assessment_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Assessments);
        }
    }
}
