using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationGet
{
    public class Then_SearchRegistrationViewModel_With_Search_Criteria_Is_Returned : When_SearchRegistration_Get_Action_Is_Called
    {
        private string _searchUln = "1234567890";
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<string>(Arg.Any<string>()).Returns(_searchUln);
        }

        [Fact]
        public void Then_Expected_Results_With_Search_Criteria_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchRegistrationViewModel));

            var model = viewResult.Model as SearchRegistrationViewModel;
            model.Should().NotBeNull();
            model.SearchUln.Should().Be(_searchUln);
        }

        [Fact]
        public void Then_Expected_Breadcrumb_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchRegistrationViewModel));

            var model = viewResult.Model as SearchRegistrationViewModel;
            model.Should().NotBeNull();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RegistrationDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Registration_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Registration);
        }
    }
}
