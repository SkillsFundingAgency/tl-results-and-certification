using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminFindProviderClearGet
{
    public class When_Called : AdminProviderControllerBaseTest
    {
        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminFindProviderClearAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminFindProviderViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminFindProvider()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminFindProvider);
        }
    }
}