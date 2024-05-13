using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationFiltersPost
{
    public class When_Called_And_Cache_Contains_Invalid_Data : SearchRegistrationControllerTestBase
    {
        private IActionResult _result;

        public override void Given()
        {
            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(null as SearchRegistrationViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationFiltersAsync(new SearchRegistrationFiltersViewModel());
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderLoader.DidNotReceive().GetProviderLookupDataAsync(Arg.Any<string>(), Arg.Any<bool>());
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<SearchRegistrationViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            _result.ShouldBeRedirectPageNotFound();
        }
    }
}