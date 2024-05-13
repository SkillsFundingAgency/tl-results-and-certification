using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.SearchRegistrationLoaderTests.CreateSearchRegistration
{
    public class When_Is_Result_Returns_Expected : TestSetup
    {
        public override void Given()
            => Given(SearchRegistrationType.Result);

        [Fact]
        public void Then_Expected_Methods_Called()
            => AssertExpectedMethodsCalled();

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.SearchType.Should().Be(SearchRegistrationType.Result);
            Result.Criteria.Filters.Should().BeEquivalentTo(FiltersViewModel);

            Result.State.Should().Be(SearchRegistrationState.NoSearch);
            Result.PageTitle.Should().Be(SearchRegistration.Page_Title_Search_Result);
            Result.PageHeading.Should().Be(SearchRegistration.Heading_Search_Result);

            Result.Breadcrumb.Should().NotBeNull();
            Result.Breadcrumb.BreadcrumbItems.Should().HaveCount(3);

            Result.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(Breadcrumb.Home);
            Result.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            Result.Breadcrumb.BreadcrumbItems[0].RouteAttributes.Should().BeEmpty();

            Result.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(Breadcrumb.Result_Dashboard);
            Result.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ResultsDashboard);
            Result.Breadcrumb.BreadcrumbItems[1].RouteAttributes.Should().BeEmpty();

            Result.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(Breadcrumb.Search_For_Result_Entry);
            Result.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNull();
            Result.Breadcrumb.BreadcrumbItems[2].RouteAttributes.Should().BeEmpty();
        }
    }
}