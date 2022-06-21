using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SubmitSearchLearnerApplyFilters
{
    public class When_Called_With_Cache_NotFound : TestSetup
    {
        private SearchLearnerFiltersViewModel _searchFilters;
        private int _academicYear;

        public override void Given()
        {
            _academicYear = 2020;
            _searchFilters = new SearchLearnerFiltersViewModel
            {
                AcademicYears = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = true },
                    new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }
                },
                IsApplyFiltersSelected = true
            };

            SearchCriteriaViewModel = new SearchCriteriaViewModel { SearchLearnerFilters = _searchFilters, AcademicYear = _academicYear };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchCriteriaViewModel>(x => x.AcademicYear == SearchCriteriaViewModel.AcademicYear && x.SearchLearnerFilters.Equals(SearchCriteriaViewModel.SearchLearnerFilters)));
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
