using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using PostResultsServiceUlnWithdrawnContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PostResultsServiceUlnWithdrawn;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PostResultsServiceUlnWithdrawn
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;
        private PostResultsServiceUlnWithdrawnViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new PostResultsServiceUlnWithdrawnViewModel { Uln = uln, Firstname = "Test", Lastname = "User", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Provider", ProviderUkprn = 985647841, TLevelTitle = "Title" };
            CacheService.GetAndRemoveAsync<PostResultsServiceUlnWithdrawnViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PostResultsServiceUlnWithdrawnViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(_mockCache.Uln);
            model.Firstname.Should().Be(_mockCache.Firstname);
            model.Lastname.Should().Be(_mockCache.Lastname);
            model.DateofBirth.Should().Be(_mockCache.DateofBirth);
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.ProviderUkprn.Should().Be(_mockCache.ProviderUkprn);
            model.TLevelTitle.Should().Be(_mockCache.TLevelTitle);

            // Uln
            model.SummaryUln.Title.Should().Be(PostResultsServiceUlnWithdrawnContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockCache.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(PostResultsServiceUlnWithdrawnContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockCache.Firstname} {_mockCache.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(PostResultsServiceUlnWithdrawnContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockCache.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(PostResultsServiceUlnWithdrawnContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockCache.ProviderName}<br/>({_mockCache.ProviderUkprn})");

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(PostResultsServiceUlnWithdrawnContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockCache.TLevelTitle);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchPostResultsService);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<PostResultsServiceUlnWithdrawnViewModel>(CacheKey);
        }
    }
}
