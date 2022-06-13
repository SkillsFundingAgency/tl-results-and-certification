using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerClearKey
{
    public class When_Cache_Found : TestSetup
    {
        private SearchCriteriaViewModel _searchCriteria;

        public override void Given()
        {
            AcademicYear = 2020;
            _searchCriteria = new SearchCriteriaViewModel { AcademicYear = AcademicYear, SearchKey = "test" };
            CacheService.GetAsync<SearchCriteriaViewModel>(CacheKey).Returns(_searchCriteria);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchCriteriaViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchCriteriaViewModel>(x => x.AcademicYear == AcademicYear && x.SearchLearnerFilters == null && x.SearchKey == null && x.IsSearchKeyApplied == false));
        }

        [Fact]
        public void Then_Redirected_To_Expected_Page()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.SearchLearnerDetails);
            route.RouteValues[Constants.AcademicYear].Should().Be(AcademicYear);
        }
    }
}
