﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddRommOutcomeSpecialismGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminAddRommOutcomeSpecialismViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddRommOutcomeSpecialismViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().GetAdminAddRommOutcomeSpecialismAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddRommOutcomeSpecialismViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}