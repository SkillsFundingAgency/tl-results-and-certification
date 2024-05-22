using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationFiltersPost
{
    public class When_Called_And_Cache_Contains_Valid_Data_No_Provider_Search : SearchRegistrationControllerTestBase
    {
        private const string PageNumberKey = "pageNumber";
        private const int PageNumber = 1;

        private SearchRegistrationFiltersViewModel _filtersViewModel;
        private SearchRegistrationCriteriaViewModel _criteriaViewModel;
        private SearchRegistrationViewModel _searchRegistrationViewModel;
        private IActionResult _result;

        public override void Given()
        {
            _filtersViewModel = new SearchRegistrationFiltersViewModel
            {
                Search = string.Empty
            };

            _criteriaViewModel = new SearchRegistrationCriteriaViewModel
            {
                SearchKey = "johnson",
                PageNumber = PageNumber
            };

            _searchRegistrationViewModel = new SearchRegistrationViewModel
            {
                Criteria = _criteriaViewModel
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_searchRegistrationViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationFiltersAsync(_filtersViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderLoader.DidNotReceive().GetProviderLookupDataAsync(Arg.Any<string>(), Arg.Any<bool>());
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchRegistrationViewModel>(
                p => p.Criteria.SearchKey == _criteriaViewModel.SearchKey
                && p.Criteria.PageNumber == _criteriaViewModel.PageNumber
                && p.Criteria.Filters.Search == _filtersViewModel.Search
                && !p.Criteria.Filters.SelectedProviderId.HasValue));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            SearchRegistrationCriteriaViewModel criteria = _searchRegistrationViewModel.Criteria;
            SearchRegistrationFiltersViewModel filters = criteria.Filters;

            criteria.Should().NotBeNull();
            criteria.SearchKey.Should().Be(_criteriaViewModel.SearchKey);
            criteria.PageNumber.Should().Be(_criteriaViewModel.PageNumber);

            filters.Search.Should().BeEmpty();
            filters.SelectedProviderId.Should().BeNull();
            filters.AcademicYears.Should().BeEquivalentTo(_filtersViewModel.AcademicYears);
        }

        [Fact]
        public void Then_Redirected_To_SearchRegistrationsRecords()
        {
            _result.ShouldBeRedirectToRouteResult(
                 RouteConstants.SearchRegistration,
                 (Constants.Type, _searchRegistrationViewModel.SearchType),
                 (PageNumberKey, _searchRegistrationViewModel.Criteria.PageNumber));
        }
    }
}