using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUlnNotFound
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;
        private RequestSoaUlnNotFoundViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new RequestSoaUlnNotFoundViewModel { Uln = uln.ToString() };
            CacheService.GetAndRemoveAsync<RequestSoaUlnNotFoundViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RequestSoaUlnNotFoundViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(uln.ToString());

            // Breadcrumb
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RequestSoaUniqueLearnerNumber);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Request_Statement_Of_Achievement);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<RequestSoaUlnNotFoundViewModel>(CacheKey);
        }
    }
}
