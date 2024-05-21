using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationClearKeyPost
{
    public class When_Called_And_Cache_Contains_Valid_Data : SearchRegistrationControllerTestBase
    {
        private const string PageNumberKey = "pageNumber";
        private const int PageNumber = 1;

        private SearchRegistrationViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = new SearchRegistrationViewModel
            {
                Criteria = new SearchRegistrationCriteriaViewModel
                {
                    SearchKey = "johnson",
                    PageNumber = PageNumber
                }
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationClearKeyAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchRegistrationViewModel>(
                p => p.Criteria.SearchKey == string.Empty
                && p.Criteria.PageNumber == PageNumber));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _viewModel.Criteria.Should().NotBeNull();
            _viewModel.Criteria.SearchKey.Should().BeEmpty();
            _viewModel.Criteria.PageNumber.Should().Be(PageNumber);
        }

        [Fact]
        public void Then_Redirected_To_SearchRegistrationsRecords()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.SearchRegistration,
                (Constants.Type, _viewModel.SearchType),
                (PageNumberKey, _viewModel.Criteria.PageNumber));
        }
    }
}