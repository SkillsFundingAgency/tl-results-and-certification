﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using PrsUlnWithdrawnContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsUlnWithdrawn;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsUlnWithdrawn
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long _uln = 1234567890;
        private PrsUlnWithdrawnViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new PrsUlnWithdrawnViewModel { Uln = _uln, Firstname = "Test", Lastname = "User", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Provider", ProviderUkprn = 985647841, TlevelTitle = "Title" };
            CacheService.GetAndRemoveAsync<PrsUlnWithdrawnViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsUlnWithdrawnViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(_mockCache.Uln);
            model.Firstname.Should().Be(_mockCache.Firstname);
            model.Lastname.Should().Be(_mockCache.Lastname);
            model.DateofBirth.Should().Be(_mockCache.DateofBirth);
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.ProviderUkprn.Should().Be(_mockCache.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockCache.TlevelTitle);

            // Uln
            model.SummaryUln.Title.Should().Be(PrsUlnWithdrawnContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockCache.Uln.ToString());


            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(PrsUlnWithdrawnContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockCache.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProviderName.Title.Should().Be(PrsUlnWithdrawnContent.Title_Provider_Name_Text);
            model.SummaryProviderName.Value.Should().Be(_mockCache.ProviderName);

            // ProviderName
            model.SummaryProviderUkprn.Title.Should().Be(PrsUlnWithdrawnContent.Title_Provider_Ukprn_Text);
            model.SummaryProviderUkprn.Value.Should().Be(_mockCache.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(PrsUlnWithdrawnContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockCache.TlevelTitle);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsSearchLearner);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<PrsUlnWithdrawnViewModel>(CacheKey);
        }
    }
}
