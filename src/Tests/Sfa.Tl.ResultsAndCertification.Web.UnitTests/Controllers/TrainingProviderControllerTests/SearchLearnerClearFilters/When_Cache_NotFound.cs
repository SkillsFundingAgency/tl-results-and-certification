using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerClearFilters
{
    public class When_Cache_NotFound : TestSetup
    {
        public override void Given()
        {
            AcademicYear = 2020;
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchCriteriaViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Is<SearchCriteriaViewModel>(x => x.AcademicYear == AcademicYear && x.SearchLearnerFilters == null && x.SearchKey == null));
        }

        [Fact]
        public void Then_Redirected_To_Expected_Page()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.SearchLearnerDetails);
            route.RouteValues["academicYear"].Should().Be(AcademicYear);
        }
    }
}
