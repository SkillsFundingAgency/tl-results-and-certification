﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsUlnWithdrawn
{
    public class When_NoCache_Found : TestSetup
    {
        private readonly PrsUlnWithdrawnViewModel _mockCache = null;
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<PrsUlnWithdrawnViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<PrsUlnWithdrawnViewModel>(CacheKey);
        }
    }
}
