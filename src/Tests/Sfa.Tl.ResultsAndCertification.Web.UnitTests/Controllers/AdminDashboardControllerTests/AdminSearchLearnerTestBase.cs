using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests
{
    public abstract class AdminSearchLearnerTestBase : AdminDashboardControllerTestBase
    {
        protected FilterLookupData CreateFilter(int id, string name, bool isSelected = false)
            => new()
            {
                Id = id,
                Name = name,
                IsSelected = isSelected
            };

        protected void AssertPaginationRouteNameAndSummary(PaginationModel pagination)
        {
            pagination.RouteName.Should().Be(RouteConstants.AdminSearchLearnersRecords);
            pagination.PaginationSummary.Should().Be(AdminSearchLearners.PaginationSummary_Text);
        }

        protected void AssertBreadcrumb(BreadcrumbModel breadcrumb)
        {
            breadcrumb.BreadcrumbItems.Should().NotBeNull().And.HaveCount(2);
            breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.AdminHome);
            breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Search_Learner_Records);
            breadcrumb.BreadcrumbItems[1].RouteName.Should().BeNullOrEmpty();
        }
    }
}