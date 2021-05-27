using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaNotAvailableNotResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;
using RequestSoaNotAvailableNoResultsContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaNotAvailableNoResults;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaNotAvailableNoResults
{
    public class When_Cache_Found : TestSetup
    {
        private RequestSoaNotAvailableNoResultsViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new RequestSoaNotAvailableNoResultsViewModel { Uln = 1234567891, LearnerName = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Provider (1234567)", TLevelTitle = "Title" };
            CacheService.GetAndRemoveAsync<RequestSoaNotAvailableNoResultsViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<RequestSoaNotAvailableNoResultsViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RequestSoaNotAvailableNoResultsViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(_mockCache.Uln);
            model.LearnerName.Should().Be(_mockCache.LearnerName);
            model.DateofBirth.Should().Be(_mockCache.DateofBirth);
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.TLevelTitle.Should().Be(_mockCache.TLevelTitle);

            // Uln
            model.SummaryUln.Title.Should().Be(RequestSoaNotAvailableNoResultsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockCache.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(RequestSoaNotAvailableNoResultsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockCache.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(RequestSoaNotAvailableNoResultsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockCache.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(RequestSoaNotAvailableNoResultsContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_mockCache.ProviderName);

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(RequestSoaNotAvailableNoResultsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockCache.TLevelTitle);

            // BackLink
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RequestSoaUniqueLearnerNumber);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}
