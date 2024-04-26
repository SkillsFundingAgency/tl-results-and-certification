using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationSearchKeyPost
{
    public class When_Called_And_Cache_Contains_Valid_Data : SearchRegistrationControllerTestBase
    {
        private const string PageNumberKey = "pageNumber";
        private const int PageNumber = 1;

        private SearchRegistrationCriteriaViewModel _criteriaViewModel;
        private SearchRegistrationViewModel _searchRegistrationViewModel;
        private IActionResult _result;

        public override void Given()
        {
            _criteriaViewModel = new SearchRegistrationCriteriaViewModel
            {
                SearchKey = "johnson",
                PageNumber = PageNumber
            };

            _searchRegistrationViewModel = new SearchRegistrationViewModel { SearchType = SearchRegistrationType.Registration };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_searchRegistrationViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationSearchKeyAsync(_criteriaViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchRegistrationViewModel>(
                p => p.Criteria.SearchKey == _criteriaViewModel.SearchKey
                && p.Criteria.PageNumber == _criteriaViewModel.PageNumber));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _searchRegistrationViewModel.Criteria.Should().NotBeNull();
            _searchRegistrationViewModel.Criteria.SearchKey.Should().Be(_criteriaViewModel.SearchKey);
            _searchRegistrationViewModel.Criteria.PageNumber.Should().Be(_criteriaViewModel.PageNumber);
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