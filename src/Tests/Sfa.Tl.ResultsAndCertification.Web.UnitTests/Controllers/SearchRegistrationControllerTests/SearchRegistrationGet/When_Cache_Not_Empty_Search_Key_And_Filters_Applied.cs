using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationGet
{
    public class When_Cache_Not_Empty_Search_Key_And_Filters_Applied : SearchRegistrationControllerTestBase
    {
        private SearchRegistrationViewModel _searchLearnerViewModel;
        private SearchRegistrationCriteriaViewModel _criteriaViewModel;
        private SearchRegistrationFiltersViewModel _filtersViewModel;
        private SearchRegistrationDetailsListViewModel _loadedSearchLearnerDetailsList;

        private readonly SearchRegistrationType _searchRegitrationType = SearchRegistrationType.Registration;
        private IActionResult _result;

        public override void Given()
        {
            _filtersViewModel = new SearchRegistrationFiltersViewModel
            {
                Search = "Shipley College",
                SelectedProviderId = 37,
                AcademicYears = new List<FilterLookupData>
                {
                    CreateFilter(2020, "2020 to 2021", isSelected: true),
                    CreateFilter(2021, "2021 to 2022"),
                    CreateFilter(2022, "2022 to 2023"),
                    CreateFilter(2023, "2023 to 2024")
                }
            };

            _criteriaViewModel = new SearchRegistrationCriteriaViewModel
            {
                SearchKey = "1234567890",
                PageNumber = 1,
                Filters = _filtersViewModel
            };

            _searchLearnerViewModel = new SearchRegistrationViewModel
            {
                Criteria = _criteriaViewModel,
                DetailsList = new SearchRegistrationDetailsListViewModel
                {
                    RegistrationDetails = new List<SearchRegistrationDetailsViewModel>()
                }
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_searchLearnerViewModel);

            _loadedSearchLearnerDetailsList = new SearchRegistrationDetailsListViewModel
            {
                RegistrationDetails = new List<SearchRegistrationDetailsViewModel>(),
                TotalRecords = 0,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    RecordFrom = 0,
                    RecordTo = 0,
                    StartPage = 1,
                    TotalItems = 9560,
                    TotalPages = 1
                }
            };

            SearchRegistrationLoader.GetSearchRegistrationDetailsListAsync(NcfeUkprn, _searchRegitrationType, _criteriaViewModel)
                .Returns(_loadedSearchLearnerDetailsList);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationAsync(_searchRegitrationType);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            SearchRegistrationLoader.Received(1).GetSearchRegistrationDetailsListAsync(NcfeUkprn, _searchRegitrationType, _criteriaViewModel);
            CacheService.Received(1).SetAsync(CacheKey, _searchLearnerViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = _result.ShouldBeViewResult<SearchRegistrationViewModel>();

            model.State.Should().Be(SearchRegistrationState.ResultsNotFound);
            model.ContainsResults.Should().BeFalse();
            model.ContainsMultipleResultsPages.Should().BeFalse();

            model.DetailsList.Should().BeEquivalentTo(_loadedSearchLearnerDetailsList);
            model.Criteria.Should().BeEquivalentTo(_criteriaViewModel);
            model.Criteria.Filters.Should().BeEquivalentTo(_filtersViewModel);
            model.Pagination.PagerInfo.Should().BeEquivalentTo(_loadedSearchLearnerDetailsList.PagerInfo);
        }
    }
}