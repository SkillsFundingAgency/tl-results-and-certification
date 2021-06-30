using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;
using RequestSoaUlnNotWithdrawnContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaUlnNotWithdrawn;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUlnNotWithdrawn
{
    public class When_Cache_Found : TestSetup
    {
        private RequestSoaUlnNotWithdrawnViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new RequestSoaUlnNotWithdrawnViewModel { Uln = 1234567891, LearnerName = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Provider (1234567)", TLevelTitle = "Title" };
            CacheService.GetAndRemoveAsync<RequestSoaUlnNotWithdrawnViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<RequestSoaUlnNotWithdrawnViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RequestSoaUlnNotWithdrawnViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(_mockCache.Uln);
            model.LearnerName.Should().Be(_mockCache.LearnerName);
            model.DateofBirth.Should().Be(_mockCache.DateofBirth);
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.TLevelTitle.Should().Be(_mockCache.TLevelTitle);

            // Uln
            model.SummaryUln.Title.Should().Be(RequestSoaUlnNotWithdrawnContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockCache.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(RequestSoaUlnNotWithdrawnContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockCache.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(RequestSoaUlnNotWithdrawnContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockCache.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(RequestSoaUlnNotWithdrawnContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_mockCache.ProviderName);

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(RequestSoaUlnNotWithdrawnContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockCache.TLevelTitle);

            // BackLink
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RequestSoaUniqueLearnerNumber);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }        
    }
}
