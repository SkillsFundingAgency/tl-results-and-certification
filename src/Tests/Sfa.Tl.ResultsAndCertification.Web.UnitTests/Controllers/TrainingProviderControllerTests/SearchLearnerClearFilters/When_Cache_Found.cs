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
    public class When_Cache_Found : TestSetup
    {
        private SearchCriteriaViewModel _searchCriteria;
        private SearchLearnerFiltersViewModel _searchFilters;

        public override void Given()
        {
            AcademicYear = 2020;
            _searchFilters = new SearchLearnerFiltersViewModel
            {
                AcademicYears = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = true },
                    new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }
                },
                IsApplyFiltersSelected = true
            };

            _searchCriteria = new SearchCriteriaViewModel { SearchLearnerFilters = _searchFilters, AcademicYear = AcademicYear };
            CacheService.GetAsync<SearchCriteriaViewModel>(CacheKey).Returns(_searchCriteria);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchCriteriaViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchCriteriaViewModel>(x => x.AcademicYear == AcademicYear && x.SearchLearnerFilters == null && x.SearchKey == null));
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
