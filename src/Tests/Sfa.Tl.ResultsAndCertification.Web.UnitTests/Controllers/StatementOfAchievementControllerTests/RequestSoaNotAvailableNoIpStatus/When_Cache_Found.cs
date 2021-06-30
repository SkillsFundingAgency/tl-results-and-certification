using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RequestSoaNotAvailableNoIpStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaNotAvailableNoIpStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaNotAvailableNoIpStatus
{
    public class When_Cache_Found : TestSetup
    {
        private RequestSoaNotAvailableNoIpStatusViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new RequestSoaNotAvailableNoIpStatusViewModel { Uln = 1234567891, LearnerName = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Provider (1234567)", TLevelTitle = "Title" };
            CacheService.GetAndRemoveAsync<RequestSoaNotAvailableNoIpStatusViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<RequestSoaNotAvailableNoIpStatusViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RequestSoaNotAvailableNoIpStatusViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(_mockCache.Uln);
            model.LearnerName.Should().Be(_mockCache.LearnerName);
            model.DateofBirth.Should().Be(_mockCache.DateofBirth);
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.TLevelTitle.Should().Be(_mockCache.TLevelTitle);

            // Uln
            model.SummaryUln.Title.Should().Be(RequestSoaNotAvailableNoIpStatusContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockCache.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(RequestSoaNotAvailableNoIpStatusContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockCache.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(RequestSoaNotAvailableNoIpStatusContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockCache.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(RequestSoaNotAvailableNoIpStatusContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_mockCache.ProviderName);

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(RequestSoaNotAvailableNoIpStatusContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockCache.TLevelTitle);

            // Breadcrumb
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Request_Statement_Of_Achievement);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RequestStatementOfAchievement);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.RequestSoaUniqueLearnerNumber);
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Statement_Of_Achievement_Not_Available);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNull();
        }
    }
}
