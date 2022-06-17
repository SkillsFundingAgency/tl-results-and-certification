using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerDetailsPost
{
    public class When_Called_With_Cache_NotFound : TestSetup
    {
        private SearchCriteriaViewModel _searchCriteria;
        private int _academicYear;

        public override void Given()
        {
            _searchCriteria = null;
            _academicYear = 2020;

            SearchCriteriaViewModel = new SearchCriteriaViewModel { AcademicYear = _academicYear, SearchKey = "test" };

            CacheService.GetAsync<SearchCriteriaViewModel>(CacheKey).Returns(_searchCriteria);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchCriteriaViewModel>(x => x.AcademicYear == _academicYear &&
                                                                                             x.SearchKey == "test" &&
                                                                                             x.IsSearchKeyApplied == true &&
                                                                                             x.SearchLearnerFilters == null));
        }

        [Fact]
        public void Then_Redirected_To_Expected_Page()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.SearchLearnerDetails);
            route.RouteValues[Constants.AcademicYear].Should().Be(_academicYear);
        }
    }
}
